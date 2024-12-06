using Microsoft.AspNetCore.Authorization;

namespace Orders.Infrastructure.Authentication;

public class RoleRequirement(string role) : IAuthorizationRequirement
{
    public string Role { get; } = role;
}