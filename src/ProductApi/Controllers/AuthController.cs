using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.DTOs;
using ProductApplication.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        /// <summary>Login (retorna access + refresh)</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            var tokens = await _auth.LoginAsync(dto, ct);
            if (tokens is null) return Unauthorized(new { error = "Credenciais inválidas." });
            return Ok(tokens);
        }

        /// <summary>Refresh (troca refresh por novos tokens)</summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto, CancellationToken ct)
        {
            var tokens = await _auth.RefreshAsync(dto, ct);
            if (tokens is null) return Unauthorized(new { error = "Refresh token inválido ou expirado." });
            return Ok(tokens);
        }

        /// <summary>Logout (revoga refresh token)</summary>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout([FromBody] RefreshRequestDto dto, CancellationToken ct)
        {
            await _auth.LogoutAsync(dto, ct);
            return NoContent();
        }
    }
}
