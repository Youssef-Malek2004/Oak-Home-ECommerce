using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orders.Application.Services.Data;
using Orders.Domain;
using Orders.Domain.Repositories;
using Orders.Infrastructure.Authentication;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence.Repositories;
using Shared.Contracts.Kafka;

namespace Orders.Infrastructure;

public static class DependencyInjection
{
    private const string DatabaseConnection = "DatabaseLocal";
    
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IOrdersDbContext, OrdersDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString(DatabaseConnection)));
        
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
    
    public static IServiceCollection ConfigureAuthenticationAndAuthorization(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
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
        
        services.AddSingleton<IAdminClientService, AdminClientService>(); //Handling Partitions and such

        return services;
    }
}