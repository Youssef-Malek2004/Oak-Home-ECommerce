using Microsoft.EntityFrameworkCore;

namespace Cart.Application.Services;

public interface ICartDbContext
{
    DbSet<Domain.Entities.Cart> Carts { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}