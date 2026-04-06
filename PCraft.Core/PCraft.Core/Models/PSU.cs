namespace PCraft.Core.Models
{
    public class PSU
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Potencia { get; set; } // Watts
        public PsuCertification Certificacao { get; set; }
    }
}
