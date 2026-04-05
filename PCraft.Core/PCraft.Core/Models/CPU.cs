namespace PCraft.Core.Models
{
    public class CPU
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public CpuSocket Socket { get; set; }
        public int TDP { get; set; } // Watts
    }
}
