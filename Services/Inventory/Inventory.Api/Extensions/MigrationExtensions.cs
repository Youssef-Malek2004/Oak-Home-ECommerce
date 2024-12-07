using Inventory.Domain.Entities;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using InventoryDbContext dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        
        dbContext.Database.Migrate();
        
        SeedData(dbContext);
    }
    
    private static void SeedData(InventoryDbContext dbContext)
    {
        if (!dbContext.Addresses.Any())
        {
            var addresses = Enumerable.Range(1, 10).Select(i => new Address
            {
                AddressId = Guid.NewGuid(),
                Street = $"Street {i}",
                City = $"City {i}",
                State = $"State {i}",
                PostalCode = $"1000{i}",
                Country = "Country",
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            }).ToList();

            dbContext.Addresses.AddRange(addresses);
            dbContext.SaveChanges();
        }

        if (!dbContext.Warehouses.Any())
        {
            // Fetch addresses for warehouses
            var addresses = dbContext.Addresses.Take(10).ToList(); // Fetch addresses client-side

            var warehouses = addresses.Select((address, index) => new Warehouse
            {
                WarehouseId = Guid.NewGuid(),
                Name = $"Warehouse {index + 1}",
                AddressId = address.AddressId,
                IsOperational = true,
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            }).ToList();

            dbContext.Warehouses.AddRange(warehouses);
            dbContext.SaveChanges();
        }
    }
}