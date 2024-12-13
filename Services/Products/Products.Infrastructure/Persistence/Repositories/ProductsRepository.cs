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
    public async Task<Result<IEnumerable<Product>>> GetProducts()
    {
        try
        {
            var products = await DB.Find<Product>().ExecuteAsync();
            return Result<IEnumerable<Product>>.Success(products);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Product>>.Failure(ProductErrors.ProductAddFailed(ex.Message));
        }
    }

    public async Task<Result<Product>> CreateProduct(CreateProductDto createProductDto, IDictionary<string, object>? dynamicFields)
    {
        try
        {
            var result = await ProductMappers.MapCreateProductDtoToProductAsync(createProductDto, dynamicFields);

            if (result.IsFailure) return result;

            var product = result.Value;

            if (product is null) return Result<Product>.Failure(ProductErrors.DoesntExist);

            await product.SaveAsync();
            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(ProductErrors.ProductAddFailed(ex.Message));
        }
    }

    public async Task<Result<Product>> GetProductById(string id)
    {
        try
        {
            var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();
            return product == null
                ? Result<Product>.Failure(ProductErrors.ProductNotFoundId(id))
                : Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(ProductErrors.ProductAddFailed(ex.Message));
        }
    }

    public async Task<Result<Product>> UpdateProduct(string id, UpdateProductDto productDto, IDictionary<string, object>? dynamicFields)
    {
        try
        {
            var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();

            if (product == null) return Result<Product>.Failure(ProductErrors.ProductNotFoundId(id));

            if (product.IsDeleted) return Result<Product>.Failure(ProductErrors.ProductUpdateFailedDeleted(id));

            ProductMappers.MapUpdateProductDtoToProduct(productDto, product);

            if (dynamicFields != null)
            {
                var product1 = product;
                var category = await DB.Find<Category>().Match(c => c.ID == product1.CategoryId).ExecuteFirstAsync();

                if (category == null)
                    return Result<Product>.Failure(ProductErrors.ProductNotFoundCategory(product.CategoryId));

                var dynamicFieldsResult = ProductMappers.MapDynamicFieldsToProduct(category, dynamicFields);

                if (dynamicFieldsResult.IsFailure) return Result<Product>.Failure(dynamicFieldsResult.Error);

                var updatedProduct = dynamicFieldsResult.Value;
                if (updatedProduct != null)
                {
                    product = ProductMappers.CopyDynamicFieldsToExistingProduct(product, updatedProduct);
                }
            }

            await DB.SaveAsync(product);
            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(ProductErrors.ProductEditFailed(ex.Message));
        }
    }

    public async Task<Result> DeleteProduct(string id)
    {
        try
        {
            var result = await DB.DeleteAsync<Product>(id);
            return result.DeletedCount == 0
                ? Result.Failure(ProductErrors.ProductRemoveFailed($"Unsuccessful deletion for Product ID: {id}"))
                : Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ProductErrors.ProductRemoveFailed(ex.Message));
        }
    }

    public async Task<Result<bool>> ToggleSoftDeleteProduct(string id)
    {
        try
        {
            var product = await DB.Find<Product>().Match(p => p.ID == id).ExecuteFirstAsync();

            if (product == null) return Result<bool>.Failure(ProductErrors.ProductNotFoundId(id));

            product.IsDeleted = !product.IsDeleted;
            product.UpdatedAt = DateTime.UtcNow;

            await DB.SaveAsync(product);
            return Result<bool>.Success(product.IsDeleted);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ProductErrors.ProductEditFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Product>>> GetProductsByCategory(string categoryId)
    {
        try
        {
            var products = await DB.Find<Product>().Match(p => p.CategoryId == categoryId).ExecuteAsync();
            return Result<IEnumerable<Product>>.Success(products);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Product>>.Failure(ProductErrors.ProductAddFailed(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<Product>>> SearchProducts(string searchTerm)
    {
        try
        {
            var lowerCaseSearchTerm = searchTerm.ToLower();

            var products = await DB.Find<Product>()
                .Match(p => p.Name.ToLower().Contains(lowerCaseSearchTerm) ||
                            p.Description.ToLower().Contains(lowerCaseSearchTerm) ||
                            p.Tags.Any(tag => tag.ToLower().Contains(lowerCaseSearchTerm)))
                .ExecuteAsync();

            return Result<IEnumerable<Product>>.Success(products);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Product>>.Failure(ProductErrors.ProductAddFailed(ex.Message));
        }
    }

    public async Task<Result<Product>> ToggleFeaturedStatus(string productId)
    {
        try
        {
            var product = await DB.Find<Product>().Match(p => p.ID == productId).ExecuteFirstAsync();

            if (product == null) return Result<Product>.Failure(ProductErrors.ProductNotFoundId(productId));

            if (product.IsDeleted)
            {
                return Result<Product>.Failure(ProductErrors.ProductToggleFeatureFailed(
                    "Product is marked as deleted and cannot have its featured status toggled."));
            }

            product.Featured = !product.Featured;
            product.UpdatedAt = DateTime.UtcNow;

            await DB.SaveAsync(product);

            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(ProductErrors.ProductEditFailed(ex.Message));
        }
    }
}
