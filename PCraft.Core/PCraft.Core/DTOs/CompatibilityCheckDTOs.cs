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
}
