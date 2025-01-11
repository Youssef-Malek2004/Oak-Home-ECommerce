using Cart.Api.Extensions;
using Cart.Api.Middlewares;
using Cart.Api.OptionsSetup;
using Cart.Infrastructure;
using Products.Api.OptionsSetup;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Configuration["ConnectionStrings:DatabaseLocal"] = 
    Environment.GetEnvironmentVariable("DATABASE_LOCAL") ?? builder.Configuration["ConnectionStrings:DatabaseLocal"];
builder.Configuration["ConnectionStrings:DatabaseDocker"] = 
    Environment.GetEnvironmentVariable("DATABASE_DOCKER") ?? builder.Configuration["ConnectionStrings:DatabaseDocker"];
builder.Configuration["UsingDocker"] = 
    Environment.GetEnvironmentVariable("USING_DOCKER") ?? builder.Configuration["UsingDocker"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CookieToJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<VendorIdMiddleware>();

var endpoints = app.MapGroup("api");

endpoints.MapGet("testing", (HttpResponse response, HttpContext context) =>
{
    response.WriteAsJsonAsync("Here" + context.Items["VendorId"] );
});


app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("Application started.");
});

app.Lifetime.ApplicationStopped.Register(() =>
{
    Console.WriteLine("Application stopped.");
});

app.UseHttpsRedirection();

app.Run();
