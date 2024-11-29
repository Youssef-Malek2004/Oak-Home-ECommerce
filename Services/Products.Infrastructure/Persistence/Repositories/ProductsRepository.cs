using Abstractions.ResultsPattern;
using MongoDB.Entities;
using Products.Application.Services;
using Products.Domain.DTOs;
using Products.Domain.Entities;
using Products.Domain.Errors;
using Products.Domain.Mappers;

namespace Products.Infrastructure.Persistence.Repositories;

public class ProductsRepository : IProductsRepository
{
    // private readonly IMongoCollection<Product>? _products = mongoDbService.Database?.GetCollection<Product>("product");

    public ProductsRepository(/*IMongoDbService mongoDbService*/)
    {
    }
    public async Task<Result<IEnumerable<Product>>> GetProducts()
    {
        var products =  await DB.Find<Product>().ExecuteAsync();
        return Result<IEnumerable<Product>>.Success(products);
    }
    
    public async Task<Result<Product>> CreateProduct(CreateProductDto createProductDto)
    {
        var product = ProductMappers.MapCreateProductDtoToProduct(createProductDto);
        
        await product.SaveAsync();
        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> GetProductById(string id)
    {
        var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();

        return product == null ? Result<Product>.Failure(ProductErrors.ProductNotFoundId(id)) : Result<Product>.Success(product);
    }

    public async Task<Result<Product>> UpdateProduct(string id, UpdateProductDto productDto)
    {
        var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();

        if (product == null)
        {
            return Result<Product>.Failure(ProductErrors.ProductNotFoundId(id));
        }
        
        ProductMappers.MapUpdateProductDtoToProduct(productDto, product);

        await DB.SaveAsync(product);
        return Result<Product>.Success(product);
    }

    
    public async Task<Result> DeleteProduct(string id)
    {
        var result = await DB.DeleteAsync<Product>(id);

        return result.DeletedCount == 0 ? Result.Failure(ProductErrors.ProductRemoveFailed("Check Product Id as unsuccessful Deletion")) : Result.Success();
    }

    public async Task<Result<IEnumerable<Product>>> GetProductsByCategory(string categoryId)
    {
        var products = await DB.Find<Product>().Match(p => p.CategoryId ==categoryId).ExecuteAsync();
        return Result<IEnumerable<Product>>.Success(products);
    }

    public async Task<Result<IEnumerable<Product>>> SearchProducts(string searchTerm)
    {
        var products = await DB.Find<Product>()
            .Match(p => p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower()))
            .ExecuteAsync();

        return Result<IEnumerable<Product>>.Success(products);
    }
}