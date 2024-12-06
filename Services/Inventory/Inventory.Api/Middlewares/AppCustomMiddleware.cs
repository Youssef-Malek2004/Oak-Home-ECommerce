namespace Inventory.Api.Middlewares;

public static class AppCustomMiddleware
{
    public static void AddLifetimeEvents(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            Console.WriteLine("Application started.");
        });

        app.Lifetime.ApplicationStopped.Register(() =>
        {
            Console.WriteLine("Application stopped.");
        });
    }
}