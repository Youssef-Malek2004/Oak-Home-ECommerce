namespace Products.Domain.Entities.Products;

public class HomeAppliancesProduct : Product
{
    public string Brand { get; set; } = null!;
    public int PowerConsumption { get; set; } //watts
    public bool IsEnergyEfficient { get; set; }
}
