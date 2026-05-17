using System.Linq;
using PCraft.Core.Models;
using PCraft.Core.DTOs;

namespace PCraft.Core.Extensions
{
    public static class BuildQueryExtensions
    {
        public static IQueryable<BuildResponseDTO> ProjetarParaDTO(this IQueryable<BuildSalva> query)
        {
            return query.Select(b => new BuildResponseDTO
            {
                Id = b.Id,
                Nome = b.Nome,
                Descricao = b.Descricao,
                Compartilhada = b.Compartilhada,
                CriadaEm = b.CriadaEm,
                Usuario = new UsuarioBuildResumoDTO
                {
                    Id = b.Usuario.Id,
                    Nome = b.Usuario.Nome ?? string.Empty
                },
                CpuId = b.CpuId,
                MotherboardId = b.MotherboardId,
                RamId = b.RamId,
                GpuId = b.GpuId,
                PsuId = b.PsuId,
                Cpu = b.Cpu != null ? b.Cpu.Nome : null,
                Motherboard = b.Motherboard != null ? b.Motherboard.Nome : null,
                Ram = b.Ram != null ? b.Ram.Nome : null,
                Gpu = b.Gpu != null ? b.Gpu.Nome : null,
                Psu = b.Psu != null ? b.Psu.Nome : null
            });
        }
    }
}
