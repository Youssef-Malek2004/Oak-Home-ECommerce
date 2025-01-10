namespace Cart.Domain.Entities;

public class CartItem
{
    public string ProductId { get; set; } = string.Empty; 
    public string ProductName { get; set; } = string.Empty; // Cached for convenience
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Cached for convenience
    public decimal TotalPrice => Quantity * UnitPrice;
}