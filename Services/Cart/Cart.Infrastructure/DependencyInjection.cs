using Cart.Application.Services;
using Cart.Infrastructure.Authentication;
using Cart.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
}