using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contracts.Authorization;

namespace Users.Infrastructure.Authentication;

public static class HasPermissionExtension
{
    public static RouteHandlerBuilder HasPermission(this RouteHandlerBuilder routeBuilder, Permissions permission)
    {
        return routeBuilder.RequireAuthorization(policyBuilder =>
        {
            policyBuilder.AddRequirements(new PermissionRequirement(permission.ToString())).Build();
        });
    }
}