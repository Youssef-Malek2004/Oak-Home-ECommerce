using Abstractions.ResultsPattern;
using MongoDB.Driver;
using MongoDB.Entities;
using Products.Application.Services;
using Products.Domain.DTOs;
using Products.Domain.Entites;

namespace Products.Infrastructure.Persistence.Repositories;

public class ProductsRepository : IProductsRepository
{
    // private readonly IMongoCollection<Product>? _products = mongoDbService.Database?.GetCollection<Product>("product");

    public ProductsRepository(IMongoDbService mongoDbService)
    {
        DB.InitAsync("demo");
    }
    public async Task<IEnumerable<Product>> GetProducts()
    {
        var products =  await DB.Find<Product>().ExecuteAsync();
        return products;
    }
    
    public async Task<Result<Product>> CreateProduct(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        await product.SaveAsync();
        return Result<Product>.Success(product);
    }
}