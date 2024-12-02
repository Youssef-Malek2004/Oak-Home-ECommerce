namespace Products.Domain.Entities.Products;

public class BookProduct : Product
{
    public string Author { get; set; } = null!;
    public string Publisher { get; set; } = null!;
    public int Pages { get; set; }
    public string Isbn { get; set; } = null!;
}
