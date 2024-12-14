using Products.Domain.Entities;

namespace Products.Domain.DTOs.ProductDtos;

public class VendorGetProductsDto
{
    public Product Product { get; set; } = null!;
    public string Category { get; set; } = null!;
}