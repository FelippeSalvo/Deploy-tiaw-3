using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class Case : Componente
    {
        [Required]
        public string Formato { get; set; } // ATX, Micro-ATX, etc.

        [Required]
        public string Cor { get; set; }

        [Required]
        public bool TemRGB { get; set; }
    }
}