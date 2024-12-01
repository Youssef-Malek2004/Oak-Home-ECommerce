using Inventory.Application.Services;
using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    private const string DatabaseConnection = "DatabaseLocal";
    
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IInventoryDbContext, InventoryDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString(DatabaseConnection)));

        return services;
    }
}