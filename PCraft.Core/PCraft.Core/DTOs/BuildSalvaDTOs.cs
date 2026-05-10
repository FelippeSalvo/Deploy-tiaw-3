using System.Text.Json.Serialization;

namespace PCraft.Core.DTOs
{
    public class CriarBuildDTO
    {
        public string Nome { get; set; } = string.Empty;
        public bool Compartilhada { get; set; }

        public int? CpuId { get; set; }
        public int? MotherboardId { get; set; }
        public int? RamId { get; set; }
        public int? GpuId { get; set; }
        public int? PsuId { get; set; }
    }

    public class EditarBuildDTO
    {
        public string Nome { get; set; } = string.Empty;
        public bool Compartilhada { get; set; }

        public int? CpuId { get; set; }
        public int? MotherboardId { get; set; }
        public int? RamId { get; set; }
        public int? GpuId { get; set; }
        public int? PsuId { get; set; }
    }

    public class BuildResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Compartilhada { get; set; }

        [JsonPropertyName("criadaEm")]
        public DateTime CriadaEm { get; set; }

        public UsuarioBuildResumoDTO Usuario { get; set; } = null!;

        public string? Cpu { get; set; }
        public string? Motherboard { get; set; }
        public string? Ram { get; set; }
        public string? Gpu { get; set; }
        public string? Psu { get; set; }
    }

    public class UsuarioBuildResumoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
