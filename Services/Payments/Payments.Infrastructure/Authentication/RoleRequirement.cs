using Microsoft.AspNetCore.Authorization;

namespace Inventory.Infrastructure.Authentication;

public class RoleRequirement(string role) : IAuthorizationRequirement
{
    public string Role { get; } = role;
}