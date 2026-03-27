using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class GPU : Componente
    {
        [Required]
        public int VRAM { get; set; } // GB

        [Required]
        public string TipoMemoria { get; set; }

        [Required]
        public int ClockCore { get; set; } // MHz

        [Required]
        public int ClockBoost { get; set; } // MHz
    }
}