using System.Text.RegularExpressions;
using Abstractions.SmartEnum;

namespace Shared.Contracts.Entities.NotificationService;

public class Groups : Enumeration<Groups>
{
    protected Groups(int value, string name) : base(value, name)
    {
    }
    
    public static readonly Groups None = new Groups(1, "None");
    public static readonly Groups Admins = new Groups(2, "Admins");
    public static readonly Groups Vendors = new Groups(3, "Vendors");
    public static readonly Groups Users = new Groups(1, "Users");
    
}