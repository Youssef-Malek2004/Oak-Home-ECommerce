namespace Orders.Domain.DTOs.Orders;

public class CreateOrderRequest
{
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }
    public string ShippingId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public List<CreateOrderItemRequest> OrderItems { get; set; } = [];
}