using Microsoft.Extensions.DependencyInjection;
using Products.Application.CQRS.CommandHandlers.Products;
using Products.Application.Services;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Persistence.Repositories;

namespace Products.Infrastructure;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IMongoDbService, MongoDbService>();
        services.AddSingleton<IProductsRepository, ProductsRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
    }
    
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateProductHandler>());
    }
}