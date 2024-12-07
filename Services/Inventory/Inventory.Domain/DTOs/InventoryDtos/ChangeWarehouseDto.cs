namespace Inventory.Domain.DTOs.InventoryDtos;

public class ChangeWarehouseDto
{
    public Guid InventoryId { get; set; }
    public Guid WarehouseId { get; set; }
}