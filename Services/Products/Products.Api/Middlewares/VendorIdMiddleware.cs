using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Products.Api.Middlewares;

public class VendorIdMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var vendorId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(vendorId))
        {
            context.Items["VendorId"] = vendorId; 
        }

        await next(context);
    }
}
