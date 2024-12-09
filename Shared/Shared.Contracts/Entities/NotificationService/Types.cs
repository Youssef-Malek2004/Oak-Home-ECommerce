using Abstractions.SmartEnum;

namespace Shared.Contracts.Entities.NotificationService;

public class Types : Enumeration<Types>
{
    protected Types(int value, string name) : base(value, name)
    {
    }
    
    public static readonly Types Info = new Types(1, "Info");
    public static readonly Types Warning = new Types(1, "Warning");
    public static readonly Types Error = new Types(1, "Error");
    public static readonly Types Success = new Types(1, "Success");
}