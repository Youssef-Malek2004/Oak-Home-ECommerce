using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Products.Application.Services;
using Products.Application.Settings;

namespace Products.Infrastructure.Persistence;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _mongoDatabase;

    public MongoDbService(IOptions<MongoSettings> mongoSettings)
    {
        var mongoUrl = MongoUrl.Create(mongoSettings.Value.ConnectionString);
        var mongoClient = new MongoClient(mongoUrl);
        _mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase? Database => _mongoDatabase;
}