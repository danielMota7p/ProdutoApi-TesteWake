namespace ProductApplication.DTOs
{
    public class ProductRequestDto
    {
        public string Nome { get; set; } = string.Empty;
        public int Estoque { get; set; }
        public decimal Valor { get; set; }
    }
}
