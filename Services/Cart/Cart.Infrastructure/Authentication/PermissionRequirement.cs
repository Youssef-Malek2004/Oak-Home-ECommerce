using Microsoft.AspNetCore.Authorization;

namespace Cart.Infrastructure.Authentication;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}