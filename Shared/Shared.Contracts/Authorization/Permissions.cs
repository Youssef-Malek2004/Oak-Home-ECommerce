using Abstractions.SmartEnum;

namespace Shared.Contracts.Authorization;

public class Permissions : Enumeration<Permissions>
{
    protected Permissions(int value, string name) : base(value, name)
    {
    }
    
    public static readonly Permissions ReadUsers = new Permissions(1, "ReadUsers");
    public static readonly Permissions UpdateUsers = new Permissions(2, "UpdateUsers");
    public static readonly Permissions SoftDeleteUsers = new Permissions(3, "SoftDeleteUsers");
    public static readonly Permissions PerformCrud = new Permissions(4, "PerformCrud");
    public static readonly Permissions MustBeSameUser = new Permissions(5, "MustBeSameUser");
    
    
}