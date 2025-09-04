using System.Linq;
using ProductApplication.DTOs;
using ProductDomain.Entities;
using ProductDomain.Interfaces;

namespace ProductApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        private static readonly HashSet<string> _sortWhitelist = new(StringComparer.OrdinalIgnoreCase)
        {
            "nome", "estoque", "valor"
        };

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : MapToResponse(entity);
        }

        public async Task<PagedResult<ProductResponseDto>> GetAsync(ProductQueryParams query, CancellationToken ct = default)
        {
            // 1) Carrega base
            var all = await _repo.GetAllAsync();

            // 2) Filtro por nome (contém)
            if (!string.IsNullOrWhiteSpace(query.Nome))
            {
                all = all.Where(p => p.Nome.Contains(query.Nome, StringComparison.OrdinalIgnoreCase));
            }

            // 3) Ordenação com whitelist
            var ordered = OrderProducts(all, query.SortBy, query.Order);

            // 4) Paginação
            var total = ordered.Count();
            var page = query.Page <= 0 ? 1 : query.Page;
            var size = query.PageSize <= 0 ? 10 : query.PageSize;

            var items = ordered
                .Skip((page - 1) * size)
                .Take(size)
                .Select(MapToResponse)
                .ToList();

            return new PagedResult<ProductResponseDto>
            {
                Items = items,
                Page = page,
                PageSize = size,
                TotalItems = total
            };
        }

        public async Task<ProductResponseDto> CreateAsync(ProductRequestDto dto, CancellationToken ct = default)
        {
            // Regras simples também no domínio
            var entity = new Product
            {
                Nome = dto.Nome.Trim(),
                Estoque = dto.Estoque,
                Valor = dto.Valor
            };

            if (!entity.EhValido())
                throw new InvalidOperationException("Produto inválido: verifique nome e valor.");

            var created = await _repo.AddAsync(entity);
            return MapToResponse(created);
        }

        public async Task<ProductResponseDto?> UpdateAsync(int id, ProductRequestDto dto, CancellationToken ct = default)
        {
            var exists = await _repo.GetByIdAsync(id);
            if (exists is null) return null;

            exists.Nome = dto.Nome.Trim();
            exists.Estoque = dto.Estoque;
            exists.Valor = dto.Valor;

            if (!exists.EhValido())
                throw new InvalidOperationException("Produto inválido: verifique nome e valor.");

            var updated = await _repo.UpdateAsync(exists);
            return MapToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            return await _repo.DeleteAsync(id);
        }

        // ------- Helpers -------

        private static IEnumerable<Product> OrderProducts(IEnumerable<Product> src, string? sortBy, string? order)
        {
            var isDesc = string.Equals(order, "desc", StringComparison.OrdinalIgnoreCase);
            var key = (sortBy ?? "nome").ToLowerInvariant();

            if (!_sortWhitelist.Contains(key))
                key = "nome";

            return key switch
            {
                "estoque" => isDesc ? src.OrderByDescending(p => p.Estoque) : src.OrderBy(p => p.Estoque),
                "valor" => isDesc ? src.OrderByDescending(p => p.Valor) : src.OrderBy(p => p.Valor),
                _ => isDesc ? src.OrderByDescending(p => p.Nome) : src.OrderBy(p => p.Nome)
            };
        }

        private static ProductResponseDto MapToResponse(Product p) => new()
        {
            Id = p.Id,
            Nome = p.Nome,
            Estoque = p.Estoque,
            Valor = p.Valor
        };
    }
}
