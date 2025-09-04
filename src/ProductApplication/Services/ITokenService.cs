using ProductDomain.Entities;

namespace ProductApplication.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, DateTime utcNow);
    }
}
