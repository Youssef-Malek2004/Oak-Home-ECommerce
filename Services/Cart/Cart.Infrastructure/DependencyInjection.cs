using Cart.Application.Services;
using Cart.Infrastructure.Authentication;
using Cart.Infrastructure.Persistence;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Contracts.Kafka;

namespace Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var usingDocker = bool.Parse(configuration.GetSection("UsingDocker").Value!);
        var databaseConnection = usingDocker ? "DatabaseDocker" : "DatabaseLocal";
        
        services.AddDbContext<ICartDbContext, CartDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString(databaseConnection)));
        
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
        
        services.AddSingleton<IAdminClientService, AdminClientService>();

        return services;
    }
}