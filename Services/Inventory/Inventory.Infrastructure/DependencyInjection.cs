using Confluent.Kafka;
using Inventory.Application.KafkaSettings;
using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
    
    public static IServiceCollection AddKafkaAdminClient(this IServiceCollection services)
    {
        
        services.AddSingleton<IAdminClient>(serviceProvider =>
        {
            var kafkaSettings = serviceProvider.GetRequiredService<IOptions<KafkaSettings>>().Value;

            var adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers
            };

            return new AdminClientBuilder(adminClientConfig).Build();
        });

        return services;
    }
}