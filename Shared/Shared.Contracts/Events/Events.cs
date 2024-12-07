using Abstractions.SmartEnum;

namespace Shared.Contracts.Events;

public class Event : Enumeration<Event>
{
    protected Event(int value, string name) : base(value, name)
    {
    }
    
    public static readonly Event Test = new Event(0, "Test");
    
    public static readonly Event ProductCreated = new Event(1, "ProductCreated");
    public static readonly Event ProductSoftDeleted = new Event(2, "ProductSoftDeleted");
    
    public static readonly Event OrderCreated = new Event(3, "OrderCreated");
    
    public static readonly Event InventoryNotEnough = new Event(4, "InventoryNotEnough");
    public static readonly Event InventoryReserved = new Event(5, "InventoryReserved");
}