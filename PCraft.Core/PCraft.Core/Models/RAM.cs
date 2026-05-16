namespace PCraft.Core.Models
{
    public class RAM : IComponent
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public RamType Tipo { get; set; }
        public int Capacidade { get; set; } // GB por módulo
        public int QuantidadeModulos { get; set; }
    }
}
