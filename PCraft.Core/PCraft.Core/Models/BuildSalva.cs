namespace PCraft.Core.Models
{
    public class BuildSalva
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Compartilhada { get; set; }
        public string? Descricao { get; set; }
        public bool Compativel { get; set; } = true;
        public DateTime CriadaEm { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int? CpuId { get; set; }
        public CPU? Cpu { get; set; }

        public int? MotherboardId { get; set; }
        public Motherboard? Motherboard { get; set; }

        public int? RamId { get; set; }
        public RAM? Ram { get; set; }

        public int? GpuId { get; set; }
        public GPU? Gpu { get; set; }

        public int? PsuId { get; set; }
        public PSU? Psu { get; set; }
    }
}
