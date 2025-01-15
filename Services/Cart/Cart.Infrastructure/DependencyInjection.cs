using Cart.Application.Services;
using Cart.Domain;
using Cart.Domain.Repositories;
using Cart.Infrastructure.Authentication;
using Cart.Infrastructure.Persistence;
using Cart.Infrastructure.Persistence.Repositories;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
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
        
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
    
    public static IServiceCollection AddAspirePersistence(this IServiceCollection services)
    {
        services.AddDbContext<ICartDbContext, CartDbContext>((serviceProvider, options) =>
        {
            var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
            
            options.UseNpgsql(dataSource);
        });

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
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

            Console.Out.WriteLine(kafkaConnectionString);

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