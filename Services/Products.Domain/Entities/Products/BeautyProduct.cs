namespace Products.Domain.Entities.Products;

public class BeautyProduct : Product
{
    public string Ingredients { get; set; } = null!; 
    public string SkinType { get; set; } = null!; 
    public int Volume { get; set; } 
}