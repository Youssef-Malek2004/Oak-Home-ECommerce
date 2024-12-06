namespace Shared.Contracts.Events.PaymentEvents;


public class PaymentProcessedEvent
{
    public string OrderId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; }
}