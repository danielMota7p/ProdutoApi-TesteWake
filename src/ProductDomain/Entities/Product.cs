namespace ProductDomain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int Estoque { get; set; }

        public decimal Valor { get; set; }

        // Regras de negócio simples:
        public bool EhValido()
        {
            return Valor >= 0 && !string.IsNullOrWhiteSpace(Nome);
        }
    }
}
