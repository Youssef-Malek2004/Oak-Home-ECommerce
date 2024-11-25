using Microsoft.AspNetCore.Authorization;

namespace Users.Infrastructure.Authentication;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}