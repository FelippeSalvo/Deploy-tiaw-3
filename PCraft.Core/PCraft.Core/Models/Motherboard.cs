using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class Motherboard : Componente
    {
        [Required]
        public string Socket{ get; set; }

        [Required]
        public string Formato { get; set; } // e.g., ATX

        [Required]
        public string Chipset { get; set; }

        [Required]
        public int SlotsRAM { get; set; }
    }
}