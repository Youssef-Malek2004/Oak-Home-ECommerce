using Abstractions.SmartEnum;

namespace Shared.Contracts.Events;

public class Event : Enumeration<Event>
{
    protected Event(int value, string name) : base(value, name)
    {
    }
    
    public static readonly Event Test = new Event(0, "Test");
    
    public static readonly Event ProductCreateRequest = new Event(1, "ProductCreateRequest");
    public static readonly Event ProductCreated = new Event(2, "ProductCreated");
    public static readonly Event ProductSoftDeleted = new Event(3, "ProductSoftDeleted");
    public static readonly Event ProductDeleteRequest = new Event(7, "ProductDeleteRequest");
    public static readonly Event ProductUpdateRequest = new Event(8, "ProductUpdateRequest");
    
    public static readonly Event OrderCreated = new Event(4, "OrderCreated");
    
    public static readonly Event InventoryNotEnough = new Event(5, "InventoryNotEnough");
    public static readonly Event InventoryReserved = new Event(6, "InventoryReserved");
    public static readonly Event InventorySupplyRequest = new Event(8, "InventorySupplyRequest");
}