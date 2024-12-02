using Abstractions.SmartEnum;

namespace Products.Domain.Entities;

public class Roles : Enumeration<Roles>
{
    protected Roles(int value, string name) : base(value, name)
    {
    }

    public static readonly Roles Registered = new Roles(1, "Registered");
    public static readonly Roles Admin = new Roles(1, "Admin");
    public static readonly Roles Vendor = new Roles(1, "Vendor");
    
}