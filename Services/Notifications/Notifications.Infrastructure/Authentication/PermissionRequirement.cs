using Microsoft.AspNetCore.Authorization;

namespace Notifications.Infrastructure.Authentication;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}