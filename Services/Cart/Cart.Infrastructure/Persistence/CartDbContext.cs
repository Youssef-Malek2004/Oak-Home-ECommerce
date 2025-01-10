using Cart.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Cart.Infrastructure.Persistence;

public class CartDbContext : DbContext, ICartDbContext
{
    
    public CartDbContext()
    {
    }

    public CartDbContext(DbContextOptions<CartDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Domain.Entities.Cart> Carts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartDbContext).Assembly);
    }

}