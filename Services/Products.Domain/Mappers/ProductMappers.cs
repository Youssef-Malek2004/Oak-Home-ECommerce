using MongoDB.Bson;
using Products.Domain.DTOs;
using Products.Domain.Entities;

namespace Products.Domain.Mappers;

public static class ProductMappers
{
    public static Product MapCreateProductDtoToProduct(CreateProductDto createProductDto)
    {
        return new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            CreatedAt = DateTime.UtcNow, 
            UpdatedAt = DateTime.UtcNow,
            CategoryId = createProductDto.CategoryId,
            Price = createProductDto.Price,
            VendorId = createProductDto.VendorId,
            ImageUrls = createProductDto.ImageUrls, 
            Sku = createProductDto.Sku,
            Tags = createProductDto.Tags ?? [] 
        };
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
}