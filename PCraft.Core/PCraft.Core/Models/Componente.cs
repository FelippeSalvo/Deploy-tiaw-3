using System.ComponentModel.DataAnnotations;

namespace PCraft.Core.Models
{
    public class Componente
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Marca { get; set; }

        [Required]
        public string Modelo { get; set; }

        [Required]
        public decimal Preco { get; set; }
    }
}