using Microsoft.AspNetCore.Authorization;

namespace Products.Infrastructure.Authentication;

public class RoleRequirement(string role) : IAuthorizationRequirement
{
    public string Role { get; } = role;
}