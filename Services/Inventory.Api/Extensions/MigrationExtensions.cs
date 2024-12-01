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
    }
}