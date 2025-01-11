using Microsoft.AspNetCore.Authorization;

namespace Cart.Infrastructure.Authentication;

public class RoleRequirement(string role) : IAuthorizationRequirement
{
    public string Role { get; } = role;
}