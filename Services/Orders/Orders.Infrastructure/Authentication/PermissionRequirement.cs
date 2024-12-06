using Microsoft.AspNetCore.Authorization;

namespace Orders.Infrastructure.Authentication;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}