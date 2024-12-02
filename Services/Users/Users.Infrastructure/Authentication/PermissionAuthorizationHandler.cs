using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Users.Application.Services;

namespace Users.Infrastructure.Authentication;

public class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync
        (AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(
            x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out Guid parsedUserId))
        {
            return;
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        var permissions = await permissionService.GetPermissionsAsync(parsedUserId);

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
                    return;
                }
            }
            
            context.Succeed(requirement);
        }
    }
}