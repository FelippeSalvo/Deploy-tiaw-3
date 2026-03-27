using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class PSU : Componente
    {
        [Required]
        public int Potencia { get; set; } // Watts

        [Required]
        public string Eficiencia { get; set; } // e.g., 80+ Bronze
    }
}