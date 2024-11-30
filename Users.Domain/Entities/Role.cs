using Abstractions.SmartEnum;

namespace Users.Domain.Entities;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Registered = new(1, "Registered");
    public static readonly Role LoggedIn = new(2, "LoggedIn");
    public static readonly Role Admin = new(3, "Admin");
    public static readonly Role Vendor = new(4, "Vendor");
    private Role(int id, string name) : base(id, name)
    {
    }

    public ICollection<Permission>? Permissions { get; set; }
}