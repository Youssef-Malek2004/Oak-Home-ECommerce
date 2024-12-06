namespace Orders.Domain.DTOs.Orders;

public class OrderItemResponse
{
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}