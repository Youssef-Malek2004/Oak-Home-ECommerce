using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;


namespace Cart.Infrastructure.Authentication;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync
        (AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(
            x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out Guid parsedUserId))
        {
            return Task.CompletedTask;
        }

        HashSet<string> permissions = context
            .User
            .Claims
            .Where(x => x.Type == "Permissions")
            .Select(x => x.Value)
            .ToHashSet();

        if (permissions.Contains(requirement.Permission))
        {
            if (requirement.Permission.Equals("MustBeSameUser"))
            {
                string? targetUserId = context.Resource switch
                {
                    HttpContext httpContext => httpContext.Request.RouteValues["id"]?.ToString(),
                    _ => null
                };
                
                if (!Guid.TryParse(targetUserId, out Guid targetParsedUserId) || targetParsedUserId != parsedUserId)
                {
                    return Task.CompletedTask;
                }
            }
            
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}