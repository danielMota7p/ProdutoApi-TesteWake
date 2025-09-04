namespace ProductApplication.DTOs
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Estoque { get; set; }
        public decimal Valor { get; set; }
    }
}
