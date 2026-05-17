using PCraft.Core.DTOs;
using PCraft.Core.Models;

namespace PCraft.Core.Extensions
{
    public static class DtoMappingExtensions
    {
        public static CPU ToEntity(this CPUDTO dto)
        {
            if (dto == null) return null;
            return new CPU
            {
                Nome = dto.Nome,
                Fabricante = dto.Fabricante,
                Socket = dto.Socket,
                TDP = dto.TDP
            };
        }

        public static Motherboard ToEntity(this MotherboardDTO dto)
        {
            if (dto == null) return null;
            return new Motherboard
            {
                Nome = dto.Nome,
                Socket = dto.Socket,
                TipoRamSuportado = dto.TipoRamSuportado,
                CapacidadeMaximaRam = dto.CapacidadeMaximaRam,
                SlotsRam = dto.SlotsRam
            };
        }

        public static RAM ToEntity(this RAMDTO dto)
        {
            if (dto == null) return null;
            return new RAM
            {
                Nome = dto.Nome,
                Tipo = dto.Tipo,
                Capacidade = dto.Capacidade,
                QuantidadeModulos = dto.QuantidadeModulos
            };
        }

        public static GPU ToEntity(this GPUDTO dto)
        {
            if (dto == null) return null;
            return new GPU
            {
                Nome = dto.Nome,
                TDP = dto.TDP,
                ConsumoRecomendado = dto.ConsumoRecomendado,
                Comprimento = dto.Comprimento
            };
        }

        public static PSU ToEntity(this PSUDTO dto)
        {
            if (dto == null) return null;
            return new PSU
            {
                Nome = dto.Nome,
                Potencia = dto.Potencia,
                Certificacao = dto.Certificacao
            };
        }
    }
}
