using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class Storage : Componente
    {
        [Required]
        public string Tipo { get; set; } // SSD, HDD

        [Required]
        public int Capacidade { get; set; } // GB

        [Required]
        public string Interface { get; set; } // SATA, NVMe

        [Required]
        public int VelocidadeLeitura { get; set; } // MB/s

        [Required]
        public int VelocidadeEscrita { get; set; } // MB/s
    }
}