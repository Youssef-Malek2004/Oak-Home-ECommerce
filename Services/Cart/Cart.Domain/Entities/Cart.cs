namespace Cart.Domain.Entities;

public class Cart
{
    public Guid CartId { get; set; } 
    public Guid UserId { get; set; } 
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal TotalPrice { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}