using Inventory.Api.Endpoints;
using Inventory.Api.Extensions;
using Inventory.Api.Middlewares;
using Inventory.Application.CQRS.EventHandlers;
using Inventory.Application.KafkaSettings;
using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Kafka;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Events;
using Shared.Contracts.Topics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<KafkaEventProcessor>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<ProductCreatedHandler>());

builder.Services.AddHostedService<KafkaInitializationHostedService>();
builder.Services.AddHostedService<KafkaHostedService>();

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

app.AddLifetimeEvents();
app.UseHttpsRedirection();

app.Run();