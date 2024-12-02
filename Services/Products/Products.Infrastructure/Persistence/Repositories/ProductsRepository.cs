using Abstractions.ResultsPattern;
using MongoDB.Entities;
using Products.Application.Services;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;
using Products.Domain.Errors;
using Products.Domain.Mappers;
using Shared.Contracts.RequestDtosAndMappers.ProductDtos;

namespace Products.Infrastructure.Persistence.Repositories;

public class ProductsRepository : IProductsRepository
{
    // private readonly IMongoCollection<Product>? _products = mongoDbService.Database?.GetCollection<Product>("product");

    // public ProductsRepository(/*IMongoDbService mongoDbService*/)
    // {
    // }
    public async Task<Result<IEnumerable<Product>>> GetProducts()
    {
        var products =  await DB.Find<Product>().ExecuteAsync();
        return Result<IEnumerable<Product>>.Success(products);
    }
    
    public async Task<Result<Product>> CreateProduct(CreateProductDto createProductDto, IDictionary<string, object>? dynamicFields)
    {
        
        var result = await ProductMappers.MapCreateProductDtoToProductAsync(createProductDto, dynamicFields);

        if (result.IsFailure) return result;
        
        var product = result.Value;
        
        if (product is null) return Result<Product>.Failure(Error.None);
        
        await product.SaveAsync();
        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> GetProductById(string id)
    {
        var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();

        return product == null ? Result<Product>.Failure(ProductErrors.ProductNotFoundId(id)) : Result<Product>.Success(product);
    }

    public async Task<Result<Product>> UpdateProduct(string id, UpdateProductDto productDto, IDictionary<string, object>? dynamicFields)
    {
        var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();

        if (product == null)
        {
            return Result<Product>.Failure(ProductErrors.ProductNotFoundId(id));
        }
        
        if (product.IsDeleted)
        {
            return Result<Product>.Failure(ProductErrors.ProductUpdateFailedDeleted(id));
        }
        
        ProductMappers.MapUpdateProductDtoToProduct(productDto, product); 

        if (dynamicFields != null)
        {
            var productNoWarning = product;
            var category = await DB.Find<Category>().Match(c => c.ID == productNoWarning.CategoryId).ExecuteFirstAsync();
            
            if (category == null)
            {
                return Result<Product>.Failure(CategoryErrors.CategoryNotFoundId(product.CategoryId));
            }

            var dynamicFieldsResult = ProductMappers.MapDynamicFieldsToProduct(category, dynamicFields);

            if (dynamicFieldsResult.IsFailure)
            {
                return Result<Product>.Failure(dynamicFieldsResult.Error);
            }

            var updatedProduct = dynamicFieldsResult.Value;

            if (updatedProduct != null)
            {
                product = ProductMappers.CopyDynamicFieldsToExistingProduct(product, updatedProduct);
            }
        }
        
        await DB.SaveAsync(product);
        return Result<Product>.Success(product);
    }

    
    public async Task<Result> DeleteProduct(string id)
    {
        var result = await DB.DeleteAsync<Product>(id);

        return result.DeletedCount == 0 ? Result.Failure(ProductErrors.ProductRemoveFailed("Check Product Id as unsuccessful Deletion")) : Result.Success();
    }
    
    public async Task<Result<bool>> ToggleSoftDeleteProduct(string id)
    {
        var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();

        if (product == null)
        {
            return Result<bool>.Failure(ProductErrors.ProductNotFoundId(id));
        }
        
        product.IsDeleted = !product.IsDeleted;
        product.UpdatedAt = DateTime.UtcNow; 
        
        await DB.SaveAsync(product);

        return Result<bool>.Success(product.IsDeleted);
    }

    public async Task<Result<IEnumerable<Product>>> GetProductsByCategory(string categoryId)
    {
        var products = await DB.Find<Product>().Match(p => p.CategoryId ==categoryId).ExecuteAsync();
        return Result<IEnumerable<Product>>.Success(products);
    }

    public async Task<Result<IEnumerable<Product>>> SearchProducts(string searchTerm)
    {
        var lowerCaseSearchTerm = searchTerm.ToLower();

        var products = await DB.Find<Product>()
            .Match(p => p.Name.ToLower().Contains(lowerCaseSearchTerm) ||
                        p.Description.ToLower().Contains(lowerCaseSearchTerm) ||
                        p.Tags.Any(tag => tag.ToLower().Contains(lowerCaseSearchTerm)))
            .ExecuteAsync();

        return Result<IEnumerable<Product>>.Success(products);
    }
    
    public async Task<Result<Product>> ToggleFeaturedStatus(string productId)
    {
        var product = await DB.Find<Product>().Match(p => p.ID == productId).ExecuteFirstAsync();

        if (product == null)
        {
            return Result<Product>.Failure(ProductErrors.ProductNotFoundId(productId));
        }
        
        if (product.IsDeleted)
        {
            return Result<Product>.Failure(ProductErrors.ProductToggleFeatureFailed("Product is marked as deleted and cannot have its featured status toggled."));
        }
        
        product.Featured = !product.Featured;
        product.UpdatedAt = DateTime.UtcNow;
        
        await DB.SaveAsync(product);

        return Result<Product>.Success(product);
    }
    
}
