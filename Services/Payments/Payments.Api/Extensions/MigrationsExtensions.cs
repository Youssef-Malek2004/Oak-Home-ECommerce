using Microsoft.EntityFrameworkCore;
using Payments.Infrastructure.Persistence;

namespace Payments.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        
        dbContext.Database.Migrate();
    }
}