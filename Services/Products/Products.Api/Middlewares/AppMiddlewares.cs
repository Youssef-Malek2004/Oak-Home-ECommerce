using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Entities;
using Products.Application.Settings;
using Products.Infrastructure.Persistence;

namespace Products.Api.Middlewares;

public static class AppMiddlewares
{
    public static IApplicationBuilder InitializeDatabaseConnection(this IApplicationBuilder app,
        bool dev, IServiceProvider serviceProvider, string? usingAspire)
    {
        var mongoSettings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
        
        if (mongoSettings.DatabaseName is null) throw new Exception("No database name supplied");

        if (usingAspire is not null && usingAspire == "true")
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ProductsDatabase");
            DB.InitAsync(mongoSettings.DatabaseName, MongoClientSettings.FromConnectionString(connectionString));
        }
        else if(dev) DB.InitAsync(mongoSettings.DatabaseName, MongoClientSettings.FromConnectionString(mongoSettings.ConnectionStringLocal));
        else DB.InitAsync(mongoSettings.DatabaseName, MongoClientSettings.FromConnectionString(mongoSettings.ConnectionStringDocker));
        
        Task.Run(Seedings.SeedCategoriesAsync).Wait();
        Task.Run(Seedings.SeedProductsAsync).Wait();
        return app;
    }   
}