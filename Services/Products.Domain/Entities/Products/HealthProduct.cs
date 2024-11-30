namespace Products.Domain.Entities.Products;

public class HealthProduct : Product
{
    public string Ingredients { get; set; } = null!; 
    public bool IsOrganic { get; set; } 
    public string UsageInstructions { get; set; } = null!;
}