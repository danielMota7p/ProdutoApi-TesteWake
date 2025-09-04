using Microsoft.Extensions.Configuration;
using ProductApplication.DTOs;
using ProductDomain.Entities;
using ProductDomain.Interfaces;
using StackExchange.Redis;

namespace ProductApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly ITokenService _tokens;
        private readonly IDatabase _redis;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository users, ITokenService tokens, IConnectionMultiplexer mux, IConfiguration config)
        {
            _users = users;
            _tokens = tokens;
            _redis = mux.GetDatabase();
            _config = config;
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto dto, CancellationToken ct = default)
        {
            var user = await _users.GetByUsernameAsync(dto.Username);
            if (user is null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            return await IssueTokensAsync(user);
        }

        public async Task<TokenResponseDto?> RefreshAsync(RefreshRequestDto dto, CancellationToken ct = default)
        {
            // refresh token é opaco (GUID) salvo no Redis -> userId
            var key = $"refresh:{dto.RefreshToken}";
            var userIdStr = await _redis.StringGetAsync(key);
            if (userIdStr.IsNullOrEmpty) return null;

            if (!int.TryParse(userIdStr!, out var userId)) return null;

            var user = await _users.GetByIdAsync(userId);
            if (user is null) return null;

            // rotação de refresh: invalida o anterior
            await _redis.KeyDeleteAsync(key);

            return await IssueTokensAsync(user);
        }

        public async Task LogoutAsync(RefreshRequestDto dto, CancellationToken ct = default)
        {
            var key = $"refresh:{dto.RefreshToken}";
            await _redis.KeyDeleteAsync(key);
        }

        private async Task<TokenResponseDto> IssueTokensAsync(User user)
        {
            var now = DateTime.UtcNow;
            var access = _tokens.GenerateAccessToken(user, now);

            var days = int.Parse(_config["Jwt:RefreshTokenDays"] ?? "7");
            var refresh = Guid.NewGuid().ToString("N");

            var key = $"refresh:{refresh}";
            // salva userId -> TTL
            await _redis.StringSetAsync(key, user.Id.ToString(), TimeSpan.FromDays(days));

            return new TokenResponseDto
            {
                AccessToken = access,
                RefreshToken = refresh,
                ExpiresAtUtc = now.AddMinutes(int.Parse(_config["Jwt:AccessTokenMinutes"] ?? "15")),
                TokenType = "Bearer"
            };
        }
    }
}
