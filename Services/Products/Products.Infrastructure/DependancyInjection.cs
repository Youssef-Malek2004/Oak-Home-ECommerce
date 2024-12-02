using Microsoft.Extensions.DependencyInjection;
using Products.Application.CQRS.CommandHandlers.Products;
using Products.Application.Services;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Products.Infrastructure.Authentication;

namespace Products.Infrastructure;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        // services.AddSingleton<IMongoDbService, MongoDbService>();
        services.AddSingleton<IProductsRepository, ProductsRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
    }
    
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateProductHandler>());
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