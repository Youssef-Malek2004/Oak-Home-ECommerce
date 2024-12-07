using Abstractions.SmartEnum;

namespace Shared.Contracts.Authorization;

public class Roles : Enumeration<Roles>
{
    protected Roles(int value, string name) : base(value, name)
    {
    }

    public static readonly Roles Registered = new Roles(1, "Registered");
    public static readonly Roles Admin = new Roles(2, "Admin");
    public static readonly Roles Vendor = new Roles(3, "Vendor");
    
}