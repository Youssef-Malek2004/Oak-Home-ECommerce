namespace Products.Domain.Entities.Products;

public class ToyProduct : Product
{
    public int MinimumAge { get; set; } 
    public string Material { get; set; } = null!; 
    public bool IsEducational { get; set; } 
}