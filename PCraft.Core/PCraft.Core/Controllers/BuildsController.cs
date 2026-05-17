using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.DTOs;
using PCraft.Core.Models;
using PCraft.Core.Extensions;

namespace PCraft.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BuildsController(AppDbContext context)
        {
            _context = context;
        }

        private bool PossuiTodosComponentes(int? cpuId, int? moboId, int? ramId, int? gpuId, int? psuId) =>
            cpuId.HasValue && moboId.HasValue && ramId.HasValue && gpuId.HasValue && psuId.HasValue;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarBuildDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new { message = "Informe um nome para a build." });

            if (!PossuiTodosComponentes(dto.CpuId, dto.MotherboardId, dto.RamId, dto.GpuId, dto.PsuId))
                return BadRequest(new { message = "Todos os 5 componentes (Processador, Placa-mãe, RAM, GPU e Fonte) são obrigatórios para salvar a build." });

            var usuarioId = User.GetUserId();
            if (usuarioId is null) return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var existeDuplicata = await _context.BuildsSalvas.AnyAsync(b => 
                b.UsuarioId == usuarioId.Value &&
                b.CpuId == dto.CpuId &&
                b.MotherboardId == dto.MotherboardId &&
                b.RamId == dto.RamId &&
                b.GpuId == dto.GpuId &&
                b.PsuId == dto.PsuId
            );

            if (existeDuplicata)
                return Conflict(new { message = "Você já possui uma build salva com esta mesma configuração de peças." });

            var build = new BuildSalva
            {
                Nome = dto.Nome.Trim(),
                Descricao = dto.Descricao?.Trim(),
                Compartilhada = dto.Compartilhada,
                UsuarioId = usuarioId.Value,
                CpuId = dto.CpuId,
                MotherboardId = dto.MotherboardId,
                RamId = dto.RamId,
                GpuId = dto.GpuId,
                PsuId = dto.PsuId,
                CriadaEm = DateTime.UtcNow
            };

            _context.BuildsSalvas.Add(build);
            await _context.SaveChangesAsync();

            var criada = await _context.BuildsSalvas.AsNoTracking().Where(b => b.Id == build.Id).ProjetarParaDTO().FirstAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<BuildResponseDTO>>> ListarMinhas()
        {
            var usuarioId = User.GetUserId();
            if (usuarioId is null) return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var lista = await _context.BuildsSalvas.AsNoTracking()
                .Where(b => b.UsuarioId == usuarioId.Value)
                .OrderByDescending(b => b.CriadaEm)
                .ProjetarParaDTO()
                .ToListAsync();

            return Ok(lista);
        }

        [HttpGet("publicas")]
        public async Task<ActionResult<IEnumerable<BuildResponseDTO>>> ListarPublicas()
        {
            var lista = await _context.BuildsSalvas.AsNoTracking()
                .Where(b => b.Compartilhada)
                .OrderByDescending(b => b.CriadaEm)
                .ProjetarParaDTO()
                .ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BuildResponseDTO>> ObterPorId(int id)
        {
            var meta = await _context.BuildsSalvas.AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new { b.UsuarioId, b.Compartilhada })
                .FirstOrDefaultAsync();
                
            if (meta is null) return NotFound(new { message = "Build não encontrada." });

            var usuarioId = User.GetUserId();
            if (!meta.Compartilhada && usuarioId != meta.UsuarioId)
                return NotFound(new { message = "Build não encontrada." });

            var dto = await _context.BuildsSalvas.AsNoTracking().Where(b => b.Id == id).ProjetarParaDTO().FirstAsync();
            return Ok(dto);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(int id, [FromBody] EditarBuildDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new { message = "Informe um nome para a build." });

            if (!PossuiTodosComponentes(dto.CpuId, dto.MotherboardId, dto.RamId, dto.GpuId, dto.PsuId))
                return BadRequest(new { message = "Todos os 5 componentes são obrigatórios." });

            var usuarioId = User.GetUserId();
            if (usuarioId is null) return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var build = await _context.BuildsSalvas.FindAsync(id);
            if (build is null) return NotFound(new { message = "Build não encontrada." });
            if (build.UsuarioId != usuarioId.Value) return Forbid();

            build.Nome = dto.Nome.Trim();
            build.Descricao = dto.Descricao?.Trim();
            build.Compartilhada = dto.Compartilhada;
            build.CpuId = dto.CpuId;
            build.MotherboardId = dto.MotherboardId;
            build.RamId = dto.RamId;
            build.GpuId = dto.GpuId;
            build.PsuId = dto.PsuId;

            await _context.SaveChangesAsync();

            var atualizada = await _context.BuildsSalvas.AsNoTracking().Where(b => b.Id == id).ProjetarParaDTO().FirstAsync();
            return Ok(atualizada);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var usuarioId = User.GetUserId();
            if (usuarioId is null) return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var build = await _context.BuildsSalvas.FindAsync(id);
            if (build is null) return NotFound(new { message = "Build não encontrada." });
            if (build.UsuarioId != usuarioId.Value) return Forbid();

            _context.BuildsSalvas.Remove(build);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
