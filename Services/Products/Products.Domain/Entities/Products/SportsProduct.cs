namespace Products.Domain.Entities.Products;

public class SportsProduct : Product
{
    public string SportType { get; set; } = null!; 
    public string Brand { get; set; } = null!;
    public string Size { get; set; } = null!; 
}