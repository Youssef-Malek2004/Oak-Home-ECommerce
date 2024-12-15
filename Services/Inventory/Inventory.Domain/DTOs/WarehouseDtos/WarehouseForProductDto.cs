namespace Inventory.Domain.DTOs.WarehouseDtos;

public class WarehouseForProductDto
{
    public Guid WarehouseId { get; set; } 
    public string Name { get; set; } = string.Empty;
    public Guid AddressId { get; set; }
    public bool IsOperational { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}