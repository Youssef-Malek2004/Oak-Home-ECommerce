using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace Products.Infrastructure.Authentication;

public class RoleAuthorizationHandler
    : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync
        (AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(
            x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out _))
        {
            return Task.CompletedTask;
        }

        var roles = context
            .User
            .Claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToHashSet();

        if (roles.Contains(requirement.Role))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}