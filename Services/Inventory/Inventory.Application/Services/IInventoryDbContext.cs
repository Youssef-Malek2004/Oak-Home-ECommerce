using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Services;

public interface IInventoryDbContext
{
    DbSet<Inventories> Inventory { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}