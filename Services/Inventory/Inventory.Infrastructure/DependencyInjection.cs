using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Repositories;
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
        
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}