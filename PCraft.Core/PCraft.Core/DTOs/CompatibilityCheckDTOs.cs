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
        public string Componente1 { get; set; }
        public string Componente2 { get; set; }
        public string Problema { get; set; }
        public string Severidade { get; set; }
    }

    public class RespostaVerificacaoCompatibilidade
    {
        public bool Compativel { get; set; }
        public List<ProblemaCompatibilidade> Problemas { get; set; }
        public int ConsumoTotalEnergia { get; set; }
        public int PSURecomendada { get; set; }
        public List<string> Recomendacoes { get; set; }
    }
}
