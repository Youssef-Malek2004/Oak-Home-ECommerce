using Abstractions.SmartEnum;

namespace Shared.Contracts.Entities.NotificationService;

public class Channels : Enumeration<Channels>
{
    protected Channels(int value, string name) : base(value, name)
    {
    }
    
    public static readonly Channels Email = new Channels(1, "Email");
    public static readonly Channels Sms = new Channels(2, "SMS");
    public static readonly Channels WebSocket = new Channels(3, "WebSocket");
}