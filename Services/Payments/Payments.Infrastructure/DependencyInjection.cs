using Confluent.Kafka;
using Inventory.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Payments.Application.Services.Data;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Persistence.Repositories;
using Shared.Contracts.Kafka;

namespace Payments.Infrastructure;

public static class DependencyInjection
{
    private const string DatabaseConnection = "DatabaseLocal";
    
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IPaymentDbContext, PaymentDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString(DatabaseConnection)));
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        return services;
    }
    
    public static IServiceCollection AddAspirePersistence(this IServiceCollection services)
    {
        services.AddDbContext<IPaymentDbContext, PaymentDbContext>((serviceProvider, options) =>
        {
            var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
            
            options.UseNpgsql(dataSource);
        });
        
        services.AddScoped<IPaymentRepository, PaymentRepository>();
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
            var kafkaConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");

            var adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = kafkaConnectionString ?? kafkaSettings.BootstrapServers
            };

            return new AdminClientBuilder(adminClientConfig).Build();
        });
        
        services.AddSingleton<IAdminClientService, AdminClientService>(); //Handling Partitions and such

        return services;
    }
}