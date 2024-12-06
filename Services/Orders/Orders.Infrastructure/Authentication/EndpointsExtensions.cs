using Microsoft.AspNetCore.Builder;

namespace Orders.Infrastructure.Authentication;

public static class EndpointsExtensions
{
    public static RouteHandlerBuilder HasPermission(this RouteHandlerBuilder routeBuilder, string permission)
    {
        return routeBuilder.RequireAuthorization(policyBuilder =>
        {
            policyBuilder.AddRequirements(new PermissionRequirement(permission)).Build();
        });
    }
    
    public static RouteHandlerBuilder HasRole(this RouteHandlerBuilder routeBuilder, string role)
    {
        return routeBuilder.RequireAuthorization(policyBuilder =>
        {
            policyBuilder.AddRequirements(new RoleRequirement(role)).Build();
        });
    }
}
