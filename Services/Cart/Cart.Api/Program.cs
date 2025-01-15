using Cart.Api.Endpoints;
using Cart.Api.Extensions;
using Cart.Api.Middlewares;
using Cart.Api.OptionsSetup;
using Cart.Infrastructure;
using Cart.Infrastructure.Kafka;
using Cart.Application.CQRS.Commands.CreateCart;
using Cart.Application.Services.Redis;
using Cart.Infrastructure.Persistence.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Kafka;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

builder.Configuration["ConnectionStrings:DatabaseLocal"] =
    Environment.GetEnvironmentVariable("DATABASE_LOCAL") ?? builder.Configuration["ConnectionStrings:DatabaseLocal"];
builder.Configuration["ConnectionStrings:DatabaseDocker"] =
    Environment.GetEnvironmentVariable("DATABASE_DOCKER") ?? builder.Configuration["ConnectionStrings:DatabaseDocker"];
builder.Configuration["UsingDocker"] =
    Environment.GetEnvironmentVariable("USING_DOCKER") ?? builder.Configuration["UsingDocker"];

var usingAspire = Environment.GetEnvironmentVariable("Using__Aspire");
if (usingAspire is not null && usingAspire == "true")
{
    builder.AddNpgsqlDataSource("CartDatabase");
    builder.Services.AddAspirePersistence();
}
else builder.Services.AddPersistence(builder.Configuration);

var aspireConnectionString = (usingAspire is not null && usingAspire == "true")
    ? Environment.GetEnvironmentVariable("ConnectionStrings__redis")
    : "";

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IKafkaNotificationService, KafkaNotificationsService>();
builder.Services.AddSingleton<KafkaEventProcessor>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateCartCommandHandler>());

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var redisSettings = provider.GetRequiredService<IOptions<RedisSettings>>().Value;

    if (aspireConnectionString != "")
    {
        return ConnectionMultiplexer.Connect(aspireConnectionString!);
    }
    return ConnectionMultiplexer.Connect(redisSettings.ConnectionStringLocal);
});

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IRedisService, RedisService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<DistributedCacheEntryOptions>(options =>
{
    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
    options.SlidingExpiration = TimeSpan.FromMinutes(20);
});

builder.Services.AddHostedService<KafkaInitializationHostedService>();
builder.Services.AddHostedService<KafkaHostedService>();

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

endpoints.MapCartEndpoints();

endpoints.MapGet("testing", (HttpResponse response, HttpContext context) =>
{
    response.WriteAsJsonAsync("Here" + context.Items["VendorId"]);
});

app.AddLifetimeEvents();
app.UseHttpsRedirection();

app.Run();
