using Inventory.Api.Extensions;
using Inventory.Application.CQRS.EventHandlers;
using Inventory.Application.KafkaSettings;
using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Kafka;
using Shared.Contracts.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<ProductCreatedHandler>());

builder.Services.AddHostedService<KafkaHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("test", async ( IKafkaProducerService kafkaProducer, CancellationToken cancellationToken ) =>
{
    await kafkaProducer.SendMessageAsync("testing-events", new TestEvent
    {
        Name = $"Sample Product {DateTime.UtcNow}",
    }, cancellationToken);
});

app.MapGet("GetInventories", async (IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
{
    var result = await unitOfWork.InventoryRepository.GetInventoriesAsync(cancellationToken);
    return Results.Ok(result.Value);
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