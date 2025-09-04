using ProductApplication.DTOs;

namespace ProductApplication.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<ProductResponseDto>> GetAsync(ProductQueryParams query, CancellationToken ct = default);
        Task<ProductResponseDto> CreateAsync(ProductRequestDto dto, CancellationToken ct = default);
        Task<ProductResponseDto?> UpdateAsync(int id, ProductRequestDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
