using Cart.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cart.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using CartDbContext dbContext = scope.ServiceProvider.GetRequiredService<CartDbContext>();
        
        dbContext.Database.Migrate();
    }
}