namespace Notifications.Api.Middlewares;

public class CookieToJwtMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]) &&
            context.Request.Cookies.TryGetValue("auth_token", out var token))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }

        await next(context);
    }
}
