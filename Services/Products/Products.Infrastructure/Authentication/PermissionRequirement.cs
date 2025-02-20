using Microsoft.AspNetCore.Authorization;

namespace Products.Infrastructure.Authentication;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}