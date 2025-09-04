using ProductApplication.DTOs;

namespace ProductApplication.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken ct = default);
        Task<TokenResponseDto?> RefreshAsync(RefreshRequestDto dto, CancellationToken ct = default);
        Task LogoutAsync(RefreshRequestDto dto, CancellationToken ct = default);
    }
}
