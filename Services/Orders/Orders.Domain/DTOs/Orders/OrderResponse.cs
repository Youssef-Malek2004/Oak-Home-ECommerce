namespace Orders.Domain.DTOs.Orders;

public class OrderResponse
{
    public Guid OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }
    public string ShippingId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public List<OrderItemResponse> OrderItems { get; set; } = new();
}