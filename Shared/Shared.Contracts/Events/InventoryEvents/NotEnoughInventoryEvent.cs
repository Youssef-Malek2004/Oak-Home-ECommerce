namespace Shared.Contracts.Events.InventoryEvents;

public class NotEnoughInventoryEvent
{
    public string OrderId { get; set; } = string.Empty;
    public List<UnavailableItem> UnavailableItems { get; set; } = new();

    public class UnavailableItem
    {
        public string ProductId { get; set; } = string.Empty;
        public int RequestedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
    }
}