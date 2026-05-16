using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.DTOs;
using PCraft.Core.Services;
using PCraft.Core.Models;
using PCraft.Core.Extensions;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompatibilidadeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICompatibilityService _compatibilityService;

        public CompatibilidadeController(AppDbContext context, ICompatibilityService compatibilityService)
        {
            _context = context;
            _compatibilityService = compatibilityService;
        }

        /// <summary>
        /// Verifica compatibilidade entre componentes (usa IDs do banco)
        /// </summary>
        [HttpPost("check")]
        public async Task<ActionResult<RespostaVerificacaoCompatibilidade>> VerificarCompatibilidade(
            [FromBody] SolicitacaoVerificacaoCompatibilidade request)
        {
            CPU cpu = request.CpuId.HasValue ? await _context.CPUs.FindAsync(request.CpuId.Value) : null;
            Motherboard motherboard = request.MotherboardId.HasValue ? await _context.Motherboards.FindAsync(request.MotherboardId.Value) : null;
            RAM ram = request.RamId.HasValue ? await _context.RAMs.FindAsync(request.RamId.Value) : null;
            GPU gpu = request.GpuId.HasValue ? await _context.GPUs.FindAsync(request.GpuId.Value) : null;
            PSU psu = request.PsuId.HasValue ? await _context.PSUs.FindAsync(request.PsuId.Value) : null;

            var ausentes = new List<string>();
            if (request.CpuId.HasValue && cpu == null) ausentes.Add("CPU");
            if (request.MotherboardId.HasValue && motherboard == null) ausentes.Add("Motherboard");
            if (request.RamId.HasValue && ram == null) ausentes.Add("RAM");
            if (request.GpuId.HasValue && gpu == null) ausentes.Add("GPU");
            if (request.PsuId.HasValue && psu == null) ausentes.Add("PSU");

            if (ausentes.Any())
                return BadRequest(new { message = "Componentes não encontrados", ausentes });

            var resultado = _compatibilityService.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);
            return Ok(resultado);
        }

        /// <summary>
        /// Verifica compatibilidade com dados inline (sem salvar no banco)
        /// </summary>
        [HttpPost("check-inline")]
        public ActionResult<RespostaVerificacaoCompatibilidade> VerificarCompatibilidadeInline(
            [FromBody] SolicitacaoVerificacaoCompatibilidadeInline request)
        {
            var cpu = request.CPU?.ToEntity();
            var motherboard = request.Motherboard?.ToEntity();
            var ram = request.RAM?.ToEntity();
            var gpu = request.GPU?.ToEntity();
            var psu = request.PSU?.ToEntity();

            var resultado = _compatibilityService.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);
            return Ok(resultado);
        }
    }
}
