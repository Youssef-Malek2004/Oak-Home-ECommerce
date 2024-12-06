namespace Shared.Contracts.Events.OrderEvents;

public class OrderCreatedEvent
{
    public string OrderId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingId { get; set; } = string.Empty;
    public List<OrderItemEvent> Items { get; set; } = [];
    public class OrderItemEvent
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}