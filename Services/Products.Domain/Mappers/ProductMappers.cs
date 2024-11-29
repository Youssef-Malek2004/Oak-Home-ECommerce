using System.Text.Json;
using Abstractions.ResultsPattern;
using MongoDB.Entities;
using Products.Domain.DTOs;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;
using Products.Domain.Entities.Products;
using Products.Domain.Errors;

namespace Products.Domain.Mappers;

public static class ProductMappers
{
    public static async Task<Result<Product>> MapCreateProductDtoToProductAsync(CreateProductDto createProductDto, IDictionary<string, object>? dynamicFields = null)
{
    var category = await DB.Find<Category>().Match(c => c.ID == createProductDto.CategoryId).ExecuteFirstAsync();
    if (category == null)
    {
        return Result<Product>.Failure(CategoryErrors.CategoryNotFoundId(createProductDto.CategoryId));
    }
    
    Product product = category.Name switch
    {
        "Electronics" => new ElectronicsProduct
        {
            Brand = dynamicFields?["Brand"]?.ToString() ?? throw new Exception("Missing field: Brand"),
            Model = dynamicFields?["Model"]?.ToString() ?? throw new Exception("Missing field: Model"),
            WarrantyPeriod = dynamicFields.TryGetValue("WarrantyPeriod", out var value) ? GetInt(value)
                : throw new Exception("Missing field: WarrantyPeriod")
        },
        "Clothing" => new ClothingProduct
        {
            Size = dynamicFields?["Size"]?.ToString() ?? throw new Exception("Missing field: Size"),
            Material = dynamicFields?["Material"]?.ToString() ?? throw new Exception("Missing field: Material"),
            Gender = dynamicFields?["Gender"]?.ToString() ?? throw new Exception("Missing field: Gender")
        },
        _ => throw new Exception($"Unsupported category: {category.Name}")
    };

    product.Name = createProductDto.Name;
    product.Description = createProductDto.Description;
    product.CreatedAt = DateTime.UtcNow;
    product.UpdatedAt = DateTime.UtcNow;
    product.CategoryId = createProductDto.CategoryId;
    product.Price = createProductDto.Price;
    product.VendorId = createProductDto.VendorId;
    product.ImageUrls = createProductDto.ImageUrls;
    product.Sku = createProductDto.Sku;
    product.Tags = createProductDto.Tags;

    return Result<Product>.Success(product);
}

    public static void MapUpdateProductDtoToProduct(UpdateProductDto productDto, Product product)
    {
        if (!string.IsNullOrEmpty(productDto.Name))
            product.Name = productDto.Name;

        if (!string.IsNullOrEmpty(productDto.Description))
            product.Description = productDto.Description;

        if (productDto.Price.HasValue)
            product.Price = productDto.Price.Value;

        if (!string.IsNullOrEmpty(productDto.Sku))
            product.Sku = productDto.Sku;

        if (!string.IsNullOrEmpty(productDto.VendorId))
            product.VendorId = productDto.VendorId;

        if (!string.IsNullOrEmpty(productDto.CategoryId))
            product.CategoryId = productDto.CategoryId;

        if (productDto.ImageUrls != null && productDto.ImageUrls.Any())
            product.ImageUrls = productDto.ImageUrls;

        product.UpdatedAt = DateTime.UtcNow;
    }
    
    private static decimal GetDecimal(object value)
    {
        if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
        {
            return jsonElement.GetDecimal();
        }
        return Convert.ToDecimal(value);
    }

    private static int GetInt(object value)
    {
        if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
        {
            return jsonElement.GetInt32();
        }
        return Convert.ToInt32(value);
    }
}