using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Notifications.Api.Middlewares;
using Notifications.Api.OptionsSetup;
using Notifications.Application.CQRS.CommandHandlers;
using Notifications.Application.Services.Redis;
using Notifications.Application.Services.SignalR;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Kafka;
using Notifications.Infrastructure.Persistence.Redis;
using Notifications.Infrastructure.SignalR;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Kafka;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var redisSettings = provider.GetRequiredService<IOptions<RedisSettings>>().Value;
    return ConnectionMultiplexer.Connect(redisSettings.ConnectionStringLocal);
});

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<KafkaEventProcessor>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddHostedService<KafkaInitializationHostedService>();
builder.Services.AddHostedService<KafkaHostedService>();


builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<MarkNotificationAsReadHandler>());

builder.Services.AddScoped<IRedisService, RedisService>();

builder.Services.AddSignalR().AddStackExchangeRedis("localhost:6379,abortConnect=false");
builder.Services.AddScoped<INotificationService, SignalRNotificationService>();
builder.Services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();


builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("test", async context =>
{
    await context.Response.WriteAsJsonAsync("testing");
});

app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) =>
{
    await context.Clients.All.ReceiveNotification(message);

    return Results.Ok();
});

app.MapPost("redis-test", async (string key, string value, IConnectionMultiplexer redis) =>
{
    var db = redis.GetDatabase();
    await db.StringSetAsync(key, value);
    var storedValue = await db.StringGetAsync(key);
    return Results.Ok(new { Key = key, Value = storedValue });
});

app.MapPost("/broadcast-random-notification", async (IHubContext<ChatHub, IChatClient> hubContext, IConnectionMultiplexer redis) =>
{
    var notification = new Notification
    {
        Title = "Random Notification",
        Message = "This is a randomly generated notification.",
        Type = "info",
        UserId = null, 
        Group = Groups.None.Name,
        Channel = Channels.WebSocket.Name,
        IsDelivered = true,
        IsRead = false,
        SentAt = DateTime.UtcNow
    };
    
    var db = redis.GetDatabase();
    var notificationKey = $"notification:{notification.Id}";

    await db.StringSetAsync(notificationKey, JsonSerializer.Serialize(notification), TimeSpan.FromDays(1)); // TTL = 1 day
    
    await hubContext.Clients.All.ReceiveNotification(JsonSerializer.Serialize(notification));

    return Results.Ok(new { Message = "Notification broadcasted and saved to Redis", Notification = notification });
});

app.MapPost("/send-notification-group/{group}", async (string group,  INotificationService notificationService) =>
{
    var notification = new Notification
    {
        Title = "Group Notification",
        Message = $"Hello, group {group}! This is a Group notification.",
        Type = "info",
        Group = group,
        Channel = Channels.WebSocket.Name,
        IsDelivered = true,
        IsRead = false,
        SentAt = DateTime.UtcNow
    };
    
    var addResult = await notificationService.SendNotificationToGroup(notification);
    if (!addResult.IsSuccess)
    {
        return Results.Problem($"Failed to save notification in Redis: {addResult.Error}");
    }

    return Results.Ok(new
    {
        Message = $"Notification sent to Group {notification.Group} and saved in Redis.",
        Notification = notification
    });
});

app.MapHub<ChatHub>("chat-hub");

app.UseMiddleware<CookieToJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.AddLifetimeEvents();

app.Run();