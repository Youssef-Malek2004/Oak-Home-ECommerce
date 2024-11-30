namespace Products.Domain.DTOs.ProductDtos;

public class UpdateProductRequest
{
    public UpdateProductDto UpdateProductDto { get; set; } = null!;
    public IDictionary<string, object>? DynamicFields { get; set; }
}