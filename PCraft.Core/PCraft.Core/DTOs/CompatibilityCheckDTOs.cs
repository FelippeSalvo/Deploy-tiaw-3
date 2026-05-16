using System.Text.Json.Serialization;

namespace PCraft.Core.DTOs
{
    public class SolicitacaoVerificacaoCompatibilidade
    {
        public int? CpuId { get; set; }
        public int? MotherboardId { get; set; }
        public int? RamId { get; set; }
        public int? GpuId { get; set; }
        public int? PsuId { get; set; }
        public int? CaseId { get; set; }
    }

    public class ProblemaCompatibilidade
    {
        [JsonPropertyName("componente1")]
        public string Componente1 { get; set; }

        [JsonPropertyName("componente2")]
        public string Componente2 { get; set; }

        [JsonPropertyName("problema")]
        public string Problema { get; set; }

        [JsonPropertyName("severidade")]
        public string Severidade { get; set; }
    }

    public class RespostaVerificacaoCompatibilidade
    {
        [JsonPropertyName("compativel")]
        public bool Compativel { get; set; }

        [JsonPropertyName("problemas")]
        public List<ProblemaCompatibilidade> Problemas { get; set; }

        [JsonPropertyName("consumoTotalEnergia")]
        public int ConsumoTotalEnergia { get; set; }

        [JsonPropertyName("psuRecomendada")]
        public int PSURecomendada { get; set; }

        [JsonPropertyName("recomendacoes")]
        public List<string> Recomendacoes { get; set; }
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
        public PCraft.Core.Models.CpuSocket Socket { get; set; }
        public int TDP { get; set; }
    }

    public class MotherboardDTO
    {
        public string Nome { get; set; }
        public PCraft.Core.Models.CpuSocket Socket { get; set; }
        public PCraft.Core.Models.RamType TipoRamSuportado { get; set; }
        public int CapacidadeMaximaRam { get; set; }
        public int SlotsRam { get; set; }
    }

    public class RAMDTO
    {
        public string Nome { get; set; }
        public PCraft.Core.Models.RamType Tipo { get; set; }
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
        public PCraft.Core.Models.PsuCertification Certificacao { get; set; }
    }
}
