namespace ProductApplication.DTOs
{
    public class ProductQueryParams
    {
        public string? Nome { get; set; }            // busca parcial por nome
        public string? SortBy { get; set; }          // "nome", "estoque", "valor"
        public string? Order { get; set; }           // "asc" ou "desc"
        public int Page { get; set; } = 1;           // paginação
        public int PageSize { get; set; } = 10;      // paginação
    }
}
