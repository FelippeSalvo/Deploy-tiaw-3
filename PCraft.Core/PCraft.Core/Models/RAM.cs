using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class RAM : Componente
    {
        [Required]
        public int Capacidade { get; set; } // GB

        [Required]
        public int Velocidade { get; set; } // MHz

        [Required]
        public string Tipo { get; set; } // e.g., DDR4
    }
}