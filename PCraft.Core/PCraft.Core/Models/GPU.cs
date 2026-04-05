namespace PCraft.Core.Models
{
    public class GPU
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int TDP { get; set; } // Watts
        public int ConsumoRecomendado { get; set; } // PSU mínima recomendada
        public int Comprimento { get; set; } // mm
    }
}
