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

        private static IQueryable<BuildSalva> QueryComPecas() =>
            _context.BuildsSalvas
                .Include(b => b.Usuario)
                .Include(b => b.Cpu)
                .Include(b => b.Motherboard)
                .Include(b => b.Ram)
                .Include(b => b.Gpu)
                .Include(b => b.Psu);

        private static BuildResponseDTO MapearResposta(BuildSalva b) => new()
        {
            Id = b.Id,
            Nome = b.Nome,
            Compartilhada = b.Compartilhada,
            CriadaEm = b.CriadaEm,
            Usuario = new UsuarioBuildResumoDTO
            {
                Id = b.Usuario.Id,
                Nome = b.Usuario.Nome
            },
            Cpu = b.Cpu?.Nome,
            Motherboard = b.Motherboard?.Nome,
            Ram = b.Ram?.Nome,
            Gpu = b.Gpu?.Nome,
            Psu = b.Psu?.Nome
        };

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

            var criada = await QueryComPecas().FirstAsync(b => b.Id == build.Id);
            return CreatedAtAction(nameof(ObterPorId), new { id = criada.Id }, MapearResposta(criada));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<BuildResponseDTO>>> ListarMinhas()
        {
            var usuarioId = ObterUsuarioIdDoToken();
            if (usuarioId is null)
                return Unauthorized(new { message = "Não foi possível identificar o usuário." });

            var lista = await QueryComPecas()
                .Where(b => b.UsuarioId == usuarioId.Value)
                .OrderByDescending(b => b.CriadaEm)
                .ToListAsync();

            return Ok(lista.Select(MapearResposta));
        }

        [HttpGet("publicas")]
        public async Task<ActionResult<IEnumerable<BuildResponseDTO>>> ListarPublicas()
        {
            var lista = await QueryComPecas()
                .Where(b => b.Compartilhada)
                .OrderByDescending(b => b.CriadaEm)
                .ToListAsync();

            return Ok(lista.Select(MapearResposta));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BuildResponseDTO>> ObterPorId(int id)
        {
            var build = await QueryComPecas().FirstOrDefaultAsync(b => b.Id == id);
            if (build is null)
                return NotFound(new { message = "Build não encontrada." });

            var usuarioId = ObterUsuarioIdDoToken();
            var podeVerPrivada = build.Compartilhada || usuarioId == build.UsuarioId;
            if (!podeVerPrivada)
                return NotFound(new { message = "Build não encontrada." });

            return Ok(MapearResposta(build));
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

            var atualizada = await QueryComPecas().FirstAsync(b => b.Id == id);
            return Ok(MapearResposta(atualizada));
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
