namespace Products.Domain.Entities.Products;

public class WoodProduct : Product
{
    public string Material { get; set; } = "Wood"; 
    public string Finish { get; set; } = null!;
    
    public decimal Length { get; set; } 
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    
    public decimal Weight { get; set; }
    public string Color { get; set; } = null!;
    public string SubCategory { get; set; } = null!;
    
    public string Usage { get; set; } = null!; // Indoor, Outdoor
    public bool IsCustomizable { get; set; } // Custom dimensions or finishes
    public string Features { get; set; } = null!; // E.g., Foldable, Extendable, Convertible
    public int WarrantyInYears { get; set; } // Warranty period
    public string MaintenanceInstructions { get; set; } = null!; // Care instructions
    
    public string Brand { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public DateTime ManufactureDate { get; set; }
    public string CountryOfOrigin { get; set; } = null!;
    
    public bool IsEcoFriendly { get; set; } // Sustainable material
}