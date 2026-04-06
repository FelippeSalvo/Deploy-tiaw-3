namespace PCraft.Core.Models
{
    public class Motherboard
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public CpuSocket Socket { get; set; }
        public RamType TipoRamSuportado { get; set; }
        public int CapacidadeMaximaRam { get; set; } // GB
        public int SlotsRam { get; set; }
    }
}
