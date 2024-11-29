namespace Products.Domain.Entities.Products;

public class ClothingProduct : Product
{
    public string Size { get; set; } = null!; // S, M, L, etc.
    public string Material { get; set; } = null!;
    public string Gender { get; set; } = null!; // Men, Women, Unisex
}
