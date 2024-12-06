using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Inventory.Application.CQRS.Commands;
using Inventory.Application.CQRS.Events;
using Inventory.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Events;
using Shared.Contracts.Events.OrderEvents;
using Shared.Contracts.Events.ProductEvents;
using Shared.Contracts.Topics;

namespace Inventory.Infrastructure.Kafka;

public class KafkaDispatcher(IKafkaConsumerService consumer, IServiceScopeFactory serviceScope)
{
    public async Task StartConsumingProductCreated(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ProductCreated>(Topics.ProductCreatedTopic.Name,
                "product-created-1",
                async message =>
            {
                using var scope = serviceScope.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                // await mediator.Send(new ProductCreatedEvent(message), stoppingToken); 
            }, stoppingToken);
            
        }, stoppingToken);
    }
    
    public async Task StartConsumingProductSoftDeleted(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ProductSoftDeleted>(Topics.ProductSoftDeleted.Name,
                "product-soft-deleted-1",
                async message =>
            {
                using var scope = serviceScope.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                // await mediator.Send(new ProductSoftDeletedEvent(message), stoppingToken); 
            }, stoppingToken);
            
        }, stoppingToken);
    }
    
    public async Task StartConsumingProductEvents(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(Topics.ProductEvents.Name, "product-events-consumer-1", async consumeResult =>
            {
                using var scope = serviceScope.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

                if (eventTypeHeader != null)
                {
                    var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
                    Console.WriteLine($"Received event type: {eventType}");

                    if (eventType == Event.ProductCreated.Name)
                    {
                        var productCreated = JsonSerializer.Deserialize<ProductCreated>(consumeResult.Message.Value);
                        if (productCreated != null)
                        {
                            Console.WriteLine($"Processing ProductCreated event: {productCreated}");
                            await mediator.Send(new ProductCreatedEvent(productCreated), stoppingToken);
                        }
                    }
                    else if (eventType == Event.ProductSoftDeleted.Name)
                    {
                        var productSoftDeleted = JsonSerializer.Deserialize<ProductSoftDeleted>(consumeResult.Message.Value);
                        if (productSoftDeleted != null)
                        {
                            Console.WriteLine($"Processing ProductSoftDeleted event: {productSoftDeleted}");
                            await mediator.Send(new ProductSoftDeletedEvent(productSoftDeleted), stoppingToken);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown event type: {eventType}");
                    }
                }
                else
                {
                    Console.WriteLine("No eventType header found.");
                }
            }, stoppingToken);
        }, stoppingToken);
    }
    
    public async Task StartConsumingOrderEvents(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(Topics.OrderEvents.Name, "order-events-consumer-1", async consumeResult =>
            {
                using var scope = serviceScope.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

                if (eventTypeHeader != null)
                {
                    var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
                    Console.WriteLine($"Received event type: {eventType}");

                    if (eventType == Event.OrderCreated.Name)
                    {
                        var orderCreated = JsonSerializer.Deserialize<OrderCreatedEvent>(consumeResult.Message.Value);
                        if (orderCreated != null)
                        {
                            Console.WriteLine($"Processing OrderCreated event: {orderCreated}");
                            await mediator.Send(new ReserveInventoryCommand(orderCreated), stoppingToken);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown event type: {eventType}");
                    }
                }
                else
                {
                    Console.WriteLine("No eventType header found.");
                }
            }, stoppingToken);
        }, stoppingToken);
    }

}