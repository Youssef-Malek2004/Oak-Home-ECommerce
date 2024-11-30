namespace Products.Domain.Entities.Products;

public class FoodAndBeverageProduct : Product
{
    public string Ingredients { get; set; } = null!; 
    public DateTime ExpiryDate { get; set; } 
    public bool IsVegetarian { get; set; } 
    public int VolumeOrWeight { get; set; } 
}