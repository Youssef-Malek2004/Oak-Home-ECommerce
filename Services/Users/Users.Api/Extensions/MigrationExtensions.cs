using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.Persistence;

namespace Users.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using UsersDbContext dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        
        dbContext.Database.Migrate();
    }
}