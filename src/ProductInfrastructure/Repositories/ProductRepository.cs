using Microsoft.EntityFrameworkCore;
using ProductDomain.Entities;
using ProductDomain.Interfaces;
using ProductInfrastructure.Data;

namespace ProductInfrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
        {
            return await _context.Produtos
                .Where(p => p.Nome.Contains(name))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Produtos.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Produtos.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Produtos.FindAsync(id);
            if (product == null) return false;

            _context.Produtos.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
