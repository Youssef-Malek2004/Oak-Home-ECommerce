using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Domain.Entities;

public class Inventories
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [MaxLength(50)]
    public string ProductId { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string VendorId { get; set; } = string.Empty; 
    
    public Guid WarehouseId { get; set; }
    
    public int Quantity { get; set; } = 0;
    
    public bool IsAvailable { get; set; } = true; 
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    [MaxLength(100)]
    public string UpdatedBy { get; set; } = "system"; 
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
}