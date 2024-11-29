namespace Products.Domain.DTOs.ProductDtos;

public class UpdateProductDto
{
    public string? Name { get; set; } 
    public string? Description { get; set; } 
    public decimal? Price { get; set; } 
    public string? Sku { get; set; } 
    public string? VendorId { get; set; } 
    public string? CategoryId { get; set; } 
    public List<string>? ImageUrls { get; set; } 
}