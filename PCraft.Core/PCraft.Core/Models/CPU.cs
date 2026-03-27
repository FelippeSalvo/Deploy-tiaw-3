using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class CPU : Componente
    {
        [Required]
        public int Nucleos { get; set; }

        [Required]
        public int Threads { get; set; }

        [Required]
        public decimal ClockBase { get; set; } // GHz

        [Required]
        public decimal ClockBoost { get; set; } // GHz

        [Required]
        public string Socket { get; set; }
    }
}