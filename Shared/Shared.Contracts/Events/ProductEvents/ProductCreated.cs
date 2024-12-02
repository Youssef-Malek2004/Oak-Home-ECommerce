namespace Shared.Contracts.Events.ProductEvents;

public class ProductCreated
{
    public string ProductId { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public decimal Price { get; set; }
}