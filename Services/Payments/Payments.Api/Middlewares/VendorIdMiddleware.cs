using System.Security.Claims;

namespace Payments.Api.Middlewares;

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
