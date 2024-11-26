using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Products.Api.Settings;
using Products.Application.Services;

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