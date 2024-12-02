namespace Products.Domain.Entities.Products;

public class AutomotiveProduct : Product
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
    public string Compatibility { get; set; } = null!; 
}