using Microsoft.AspNetCore.Authorization;

namespace Inventory.Infrastructure.Authentication;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}