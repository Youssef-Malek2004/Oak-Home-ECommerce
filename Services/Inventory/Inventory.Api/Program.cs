using Inventory.Api.Endpoints;
using Inventory.Api.Extensions;
using Inventory.Api.Middlewares;
using Inventory.Api.OptionsSetup;
using Inventory.Application.CQRS.EventHandlers;
using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Kafka;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration["ConnectionStrings:DatabaseLocal"] = 
    Environment.GetEnvironmentVariable("DATABASE_LOCAL") ?? builder.Configuration["ConnectionStrings:DatabaseLocal"];
builder.Configuration["ConnectionStrings:DatabaseDocker"] = 
    Environment.GetEnvironmentVariable("DATABASE_DOCKER") ?? builder.Configuration["ConnectionStrings:DatabaseDocker"];
builder.Configuration["UsingDocker"] = 
    Environment.GetEnvironmentVariable("USING_DOCKER") ?? builder.Configuration["UsingDocker"];

builder.Services.AddPersistence(builder.Configuration);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IKafkaNotificationService, KafkaNotificationsService>();
builder.Services.AddSingleton<KafkaEventProcessor>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<ProductCreatedHandler>());

builder.Services.AddHostedService<KafkaInitializationHostedService>();
builder.Services.AddHostedService<KafkaHostedService>();

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

app.MapGet("GetInventories", async (IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
{
    var result = await unitOfWork.InventoryRepository.GetInventoriesAsync(cancellationToken);
    return Results.Ok(result.Value);
});

app.MapPut("update", async ([FromBody]Inventories inventories,IUnitOfWork unitOfWork) =>
{
    await unitOfWork.InventoryRepository.UpdateInventoryAsync(inventories);
    await unitOfWork.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("test-event/{number:int}", async (int number, IKafkaProducerService kafkaProducerService, CancellationToken cancellationToken) =>
{
    for (int i = 0; i < number; i++) 
    {
        await kafkaProducerService.SendMessageAsync(
            Topics.TestingTopic.Name,
            new TestEvent { Number = i },
            cancellationToken,
            Event.Test.Name);
    }

    return Results.Ok();
});

var endpoints = app.MapGroup("api");

endpoints.MapInventoryEndpoints();
endpoints.MapWarehouseEndpoints();
endpoints.MapVendorEndpoints();

app.UseMiddleware<CookieToJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.AddLifetimeEvents();
app.UseHttpsRedirection();

app.Run();