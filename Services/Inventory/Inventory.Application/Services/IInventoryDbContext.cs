using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Services;

public interface IInventoryDbContext
{
    DbSet<Inventories> Inventory { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}