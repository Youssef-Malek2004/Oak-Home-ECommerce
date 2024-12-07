using System.ComponentModel.DataAnnotations;

namespace Inventory.Domain.Entities;

public class Warehouse
{
    [Key]
    public Guid WarehouseId { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public Guid AddressId { get; set; }
    public Address Address { get; set; } = default!; 

    public bool IsOperational { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public ICollection<Inventories> Inventories { get; set; } = new List<Inventories>();
}
