using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCraft.Core.Data;
using PCraft.Core.DTOs;
using PCraft.Core.Models;

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

        private async Task<List<BuildResponseDTO>> ProjecaoListaAsync(IQueryable<BuildSalva> builds)
        {
            var rows = await builds
                .Select(b => new
                {
                    b.Id,
                    b.Nome,
                    b.Compartilhada,
                    b.CriadaEm,
                    UsuarioId = b.Usuario.Id,
                    UsuarioNome = b.Usuario.Nome,
                    b.CpuId,
                    b.MotherboardId,
                    b.RamId,
                    b.GpuId,
                    b.PsuId,
                    CpuNome = b.Cpu != null ? b.Cpu.Nome : null,
                    MotherboardNome = b.Motherboard != null ? b.Motherboard.Nome : null,
                    RamNome = b.Ram != null ? b.Ram.Nome : null,
                    GpuNome = b.Gpu != null ? b.Gpu.Nome : null,
                    PsuNome = b.Psu != null ? b.Psu.Nome : null
                })
                .ToListAsync();

            return rows.Select(r => new BuildResponseDTO
            {
                Id = r.Id,
                Nome = r.Nome,
                Compartilhada = r.Compartilhada,
                CriadaEm = r.CriadaEm,
                Usuario = new UsuarioBuildResumoDTO
                {
                    Id = r.UsuarioId,
                    Nome = r.UsuarioNome ?? string.Empty
                },
                CpuId = r.CpuId,
                MotherboardId = r.MotherboardId,
                RamId = r.RamId,
                GpuId = r.GpuId,
                PsuId = r.PsuId,
                Cpu = r.CpuNome,
                Motherboard = r.MotherboardNome,
                Ram = r.RamNome,
                Gpu = r.GpuNome,
                Psu = r.PsuNome
            }).ToList();
        }

        private int? ObterUsuarioIdDoToken()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(sub, out var id) ? id : null;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarBuildDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new { message = "Informe um nome para a build." });

            if (!dto.CpuId.HasValue && !dto.MotherboardId.HasValue && !dto.RamId.HasValue &&
                !dto.GpuId.HasValue && !dto.PsuId.HasValue)
            {
                return BadRequest(new { message = "Selecione pelo menos um componente." });
            }

            var usuarioId = ObterUsuarioIdDoToken();
            if (usuarioId is null)
                return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var build = new BuildSalva
            {
                Nome = dto.Nome.Trim(),
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

            var criada = (await ProjecaoListaAsync(_context.BuildsSalvas.AsNoTracking().Where(b => b.Id == build.Id)))
                .First();
            return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, criada);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<BuildResponseDTO>>> ListarMinhas()
        {
            var usuarioId = ObterUsuarioIdDoToken();
            if (usuarioId is null)
                return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var lista = await ProjecaoListaAsync(
                _context.BuildsSalvas.AsNoTracking()
                    .Where(b => b.UsuarioId == usuarioId.Value)
                    .OrderByDescending(b => b.CriadaEm));

            return Ok(lista);
        }

        [HttpGet("publicas")]
        public async Task<ActionResult<IEnumerable<BuildResponseDTO>>> ListarPublicas()
        {
            var lista = await ProjecaoListaAsync(
                _context.BuildsSalvas.AsNoTracking()
                    .Where(b => b.Compartilhada)
                    .OrderByDescending(b => b.CriadaEm));

            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BuildResponseDTO>> ObterPorId(int id)
        {
            var meta = await _context.BuildsSalvas.AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new { b.UsuarioId, b.Compartilhada })
                .FirstOrDefaultAsync();
            if (meta is null)
                return NotFound(new { message = "Build não encontrada." });

            var usuarioId = ObterUsuarioIdDoToken();
            if (!meta.Compartilhada && usuarioId != meta.UsuarioId)
                return NotFound(new { message = "Build não encontrada." });

            var dto = (await ProjecaoListaAsync(_context.BuildsSalvas.AsNoTracking().Where(b => b.Id == id)))
                .First();
            return Ok(dto);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(int id, [FromBody] EditarBuildDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(new { message = "Informe um nome para a build." });

            if (!dto.CpuId.HasValue && !dto.MotherboardId.HasValue && !dto.RamId.HasValue &&
                !dto.GpuId.HasValue && !dto.PsuId.HasValue)
            {
                return BadRequest(new { message = "Mantenha pelo menos um componente." });
            }

            var usuarioId = ObterUsuarioIdDoToken();
            if (usuarioId is null)
                return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var build = await _context.BuildsSalvas.FindAsync(id);
            if (build is null)
                return NotFound(new { message = "Build não encontrada." });

            if (build.UsuarioId != usuarioId.Value)
                return Forbid();

            build.Nome = dto.Nome.Trim();
            build.Compartilhada = dto.Compartilhada;
            build.CpuId = dto.CpuId;
            build.MotherboardId = dto.MotherboardId;
            build.RamId = dto.RamId;
            build.GpuId = dto.GpuId;
            build.PsuId = dto.PsuId;

            await _context.SaveChangesAsync();

            var atualizada = (await ProjecaoListaAsync(_context.BuildsSalvas.AsNoTracking().Where(b => b.Id == id)))
                .First();
            return Ok(atualizada);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var usuarioId = ObterUsuarioIdDoToken();
            if (usuarioId is null)
                return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var build = await _context.BuildsSalvas.FindAsync(id);
            if (build is null)
                return NotFound(new { message = "Build não encontrada." });

            if (build.UsuarioId != usuarioId.Value)
                return Forbid();

            _context.BuildsSalvas.Remove(build);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
