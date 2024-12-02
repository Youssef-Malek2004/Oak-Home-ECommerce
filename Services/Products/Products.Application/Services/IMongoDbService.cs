using MongoDB.Driver;

namespace Products.Application.Services;

public interface IMongoDbService
{
    public IMongoDatabase? Database { get; }
}