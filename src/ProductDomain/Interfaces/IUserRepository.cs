using ProductDomain.Entities;

namespace ProductDomain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
    }
}
