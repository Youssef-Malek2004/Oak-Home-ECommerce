namespace Products.Domain.DTOs.ProductDtos;

public class CreateProductRequest
{
    public CreateProductDto CreateProductDto { get; set; } = null!;
    public IDictionary<string, object>? DynamicFields { get; set; }
}
