namespace Shared.Contracts.Events.InventoryEvents;

public class InventoryReservedEvent
{
    public string OrderId { get; set; } = string.Empty;
    public List<ReservedItem> ReservedItems { get; set; } = [];

    public class ReservedItem
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}