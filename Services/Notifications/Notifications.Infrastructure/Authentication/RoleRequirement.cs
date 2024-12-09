using Microsoft.AspNetCore.Authorization;

namespace Notifications.Infrastructure.Authentication;

public class RoleRequirement(string role) : IAuthorizationRequirement
{
    public string Role { get; } = role;
}