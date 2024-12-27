namespace Payments.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; } = new Guid(); 
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; } 
    public DateTime PaymentTime { get; set; }
    public decimal Amount { get; set; } // Amount paid
    public string Status { get; set; } = string.Empty; // Payment status (successful, failed, pending)
    public string PaymentMethod { get; set; } = string.Empty; // Method of payment (e.g., Card, PayPal)
    public string TransactionId { get; set; } = string.Empty; // External payment gateway transaction ID
    public string Currency { get; set; } = string.Empty; // Currency code (e.g., USD, EUR)
    public string? FailureReason { get; set; } = string.Empty; // Reason for failure, if any
}