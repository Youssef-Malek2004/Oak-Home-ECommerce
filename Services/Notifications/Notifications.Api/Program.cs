using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Notifications.Api.SignalR;
using Notifications.Application.CQRS.CommandHandlers;
using Notifications.Application.Services.Redis;
using Notifications.Application.Services.SignalR;
using Notifications.Domain.Entities;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Kafka;
using Notifications.Infrastructure.Persistence.Redis;
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
        Group = "General",
        Channel = "WebSocket",
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

app.MapPost("/send-notification/{userId}", async (string userId, IHubContext<ChatHub, IChatClient> hubContext, IRedisService redisService) =>
{
    if (!Guid.TryParse(userId, out var userGuid))
    {
        return Results.BadRequest("Invalid userId format.");
    }

    var notification = new Notification
    {
        Title = "Personal Notification",
        Message = $"Hello, user {userId}! This is a personal notification.",
        Type = "info",
        UserId = userGuid,
        Group = "Personal",
        Channel = "WebSocket",
        IsDelivered = true,
        IsRead = false,
        SentAt = DateTime.UtcNow
    };
    
    var addResult = await redisService.AddNotificationAsync(userGuid, notification);
    if (!addResult.IsSuccess)
    {
        return Results.Problem($"Failed to save notification in Redis: {addResult.Error}");
    }
    
    try
    {
        await hubContext.Clients.Group(userId).ReceiveNotification(JsonSerializer.Serialize(notification));
    }
    catch (Exception ex)
    {
        return Results.Problem($"Failed to send notification via SignalR: {ex.Message}");
    }

    return Results.Ok(new
    {
        Message = $"Notification sent to user {userId} and saved in Redis.",
        Notification = notification
    });
});

app.MapHub<ChatHub>("chat-hub");

app.Run();