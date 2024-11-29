using MongoDB.Entities;
using Products.Infrastructure.Persistence;

namespace Products.Api.Middlewares;

public static class AppMiddlewares
{
    public static IApplicationBuilder InitializeDatabaseConnection(this IApplicationBuilder app)
    {
        DB.InitAsync("ECommerce-Products");
        Task.Run(Seedings.SeedCategoriesAsync).Wait();
        return app;
    }   
}