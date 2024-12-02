using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Services;

public interface IInventoryDbContext
{
    DbSet<Domain.Entities.Inventory> Inventory { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}