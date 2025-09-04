using Microsoft.EntityFrameworkCore;
using ProductDomain.Entities;
using ProductDomain.Interfaces;
using ProductInfrastructure.Data;

namespace ProductInfrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _ctx;
        public UserRepository(AppDbContext ctx) => _ctx = ctx;

        public Task<User?> GetByUsernameAsync(string username) =>
            _ctx.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

        public Task<User?> GetByIdAsync(int id) =>
            _ctx.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User> AddAsync(User user)
        {
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            return user;
        }
    }
}
