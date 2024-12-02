namespace Products.Domain.Entities.Products;

public class ElectronicsProduct : Product
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int WarrantyPeriod { get; set; } 
}
