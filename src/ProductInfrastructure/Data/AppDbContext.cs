using Microsoft.EntityFrameworkCore;
using ProductDomain.Entities;

namespace ProductInfrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Produtos { get; set; }
        public DbSet<User> Users { get; set; } // <-- add

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Estoque).IsRequired();
                entity.Property(p => p.Valor).IsRequired().HasColumnType("decimal(18,2)");
            });

            // Users
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Username).IsUnique();
                e.Property(u => u.Username).IsRequired().HasMaxLength(100);
                e.Property(u => u.PasswordHash).IsRequired();
                e.Property(u => u.Role).IsRequired().HasMaxLength(30);
            });

            // Seed 5 produtos
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Nome = "Bola de Futebol", Estoque = 50, Valor = 99.90m },
                new Product { Id = 2, Nome = "Tênis de Corrida", Estoque = 20, Valor = 299.90m },
                new Product { Id = 3, Nome = "Camiseta Esportiva", Estoque = 100, Valor = 59.90m },
                new Product { Id = 4, Nome = "Raquete de Tênis", Estoque = 15, Valor = 499.90m },
                new Product { Id = 5, Nome = "Luvas de Boxe", Estoque = 10, Valor = 199.90m }
            );

          
        }
    }
}
