using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.DTOs;
using PCraft.Core.Services;
using PCraft.Core.Models;

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
            var cpu = request.CPU == null ? null : new CPU
            {
                Nome = request.CPU.Nome,
                Fabricante = request.CPU.Fabricante,
                Socket = request.CPU.Socket,
                TDP = request.CPU.TDP
            };

            var motherboard = request.Motherboard == null ? null : new Motherboard
            {
                Nome = request.Motherboard.Nome,
                Socket = request.Motherboard.Socket,
                TipoRamSuportado = request.Motherboard.TipoRamSuportado,
                CapacidadeMaximaRam = request.Motherboard.CapacidadeMaximaRam,
                SlotsRam = request.Motherboard.SlotsRam
            };

            var ram = request.RAM == null ? null : new RAM
            {
                Nome = request.RAM.Nome,
                Tipo = request.RAM.Tipo,
                Capacidade = request.RAM.Capacidade,
                QuantidadeModulos = request.RAM.QuantidadeModulos
            };

            var gpu = request.GPU == null ? null : new GPU
            {
                Nome = request.GPU.Nome,
                TDP = request.GPU.TDP,
                ConsumoRecomendado = request.GPU.ConsumoRecomendado,
                Comprimento = request.GPU.Comprimento
            };

            var psu = request.PSU == null ? null : new PSU
            {
                Nome = request.PSU.Nome,
                Potencia = request.PSU.Potencia,
                Certificacao = request.PSU.Certificacao
            };

            var resultado = _compatibilityService.VerificarCompatibilidade(cpu, motherboard, ram, gpu, psu);
            return Ok(resultado);
        }
    }

    public class SolicitacaoVerificacaoCompatibilidadeInline
    {
        public CPUDTO CPU { get; set; }
        public MotherboardDTO Motherboard { get; set; }
        public RAMDTO RAM { get; set; }
        public GPUDTO GPU { get; set; }
        public PSUDTO PSU { get; set; }
    }

    public class CPUDTO
    {
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public CpuSocket Socket { get; set; }
        public int TDP { get; set; }
    }

    public class MotherboardDTO
    {
        public string Nome { get; set; }
        public CpuSocket Socket { get; set; }
        public RamType TipoRamSuportado { get; set; }
        public int CapacidadeMaximaRam { get; set; }
        public int SlotsRam { get; set; }
    }

    public class RAMDTO
    {
        public string Nome { get; set; }
        public RamType Tipo { get; set; }
        public int Capacidade { get; set; }
        public int QuantidadeModulos { get; set; }
    }

    public class GPUDTO
    {
        public string Nome { get; set; }
        public int TDP { get; set; }
        public int ConsumoRecomendado { get; set; }
        public int Comprimento { get; set; }
    }

    public class PSUDTO
    {
        public string Nome { get; set; }
        public int Potencia { get; set; }
        public PsuCertification Certificacao { get; set; }
    }
}
