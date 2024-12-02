using Inventory.Api.Extensions;
using Inventory.Application.KafkaSettings;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Kafka;
using Shared.Contracts.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<KafkaProducerService>();
builder.Services.AddSingleton<KafkaConsumerService>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddHostedService<KafkaHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("test", async (HttpContext context, HttpRequest request, KafkaProducerService kafkaProducer, CancellationToken cancellationToken ) =>
{
    await kafkaProducer.SendMessageAsync("testing-events", new TestEvent
    {
        Name = $"Sample Product {DateTime.UtcNow}",
    }, cancellationToken);
    
    // await kafkaProducer.SendMessageAsync("testing-events", new Test2
    // {
    //     Name = $"Sample Product 2 {DateTime.UtcNow}",
    //     Num = 2
    // }, cancellationToken);
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