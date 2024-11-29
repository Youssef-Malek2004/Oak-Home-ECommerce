using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Products.Domain.Entities;

namespace Products.Domain.DTOs;

public class CreateProductDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string CategoryId { get; set; } // Required for product categorization
    public string VendorId { get; set; } = string.Empty; // Vendor linking
    public List<string> ImageUrls { get; set; } = []; // Optional: Images for the product
    public string Sku { get; set; } = null!; // SKU for product identification
    public decimal Price { get; set; } // Product price
    public List<string> Tags { get; set; } = [];
}