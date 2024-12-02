namespace Shared.Contracts.Events.ProductEvents;

public class ProductSoftDeleted 
{
    public string ProductId { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}