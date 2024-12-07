using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Inventory.Application.CQRS.Commands;
using Inventory.Application.CQRS.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Events;
using Shared.Contracts.Events.OrderEvents;
using Shared.Contracts.Events.ProductEvents;

namespace Inventory.Infrastructure.Kafka;

public class KafkaEventProcessor(IServiceScopeFactory serviceScope)
{
    public async Task ProcessProductEvent(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
    {
        using var scope = serviceScope.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

        if (eventTypeHeader == null)
        {
            Console.WriteLine("No eventType header found.");
            return;
        }

        var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
        Console.WriteLine($"Received event type: {eventType}");

        if (eventType == Event.ProductCreated.Name)
        {
            var productCreated = JsonSerializer.Deserialize<ProductCreated>(consumeResult.Message.Value);
            if (productCreated != null)
            {
                Console.WriteLine($"Processing ProductCreated event: {productCreated}");
                await mediator.Send(new ProductCreatedEvent(productCreated), cancellationToken);
            }
        }
        else if (eventType == Event.ProductSoftDeleted.Name)
        {
            var productSoftDeleted = JsonSerializer.Deserialize<ProductSoftDeleted>(consumeResult.Message.Value);
            if (productSoftDeleted != null)
            {
                Console.WriteLine($"Processing ProductSoftDeleted event: {productSoftDeleted}");
                await mediator.Send(new ProductSoftDeletedEvent(productSoftDeleted), cancellationToken);
            }
        }
        else
        {
            Console.WriteLine($"Unknown event type: {eventType}");
        }
    }

    public async Task ProcessOrderEvent(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
    {
        using var scope = serviceScope.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

        if (eventTypeHeader == null)
        {
            Console.WriteLine("No eventType header found.");
            return;
        }

        var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
        Console.WriteLine($"Received event type: {eventType}");

        if (eventType == Event.OrderCreated.Name)
        {
            var orderCreated = JsonSerializer.Deserialize<OrderCreatedEvent>(consumeResult.Message.Value);
            if (orderCreated != null)
            {
                Console.WriteLine($"Processing OrderCreated event: {orderCreated}");
                await mediator.Send(new ReserveInventoryCommand(orderCreated), cancellationToken);
            }
        }
        else
        {
            Console.WriteLine($"Unknown event type: {eventType}");
        }
    }
    
    public Task ProcessTestEvent(ConsumeResult<string, string> consumeResult, string groupInstanceName, CancellationToken cancellationToken)
    {

        var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

        if (eventTypeHeader == null)
        {
            Console.WriteLine("No eventType header found.");
            return Task.CompletedTask;
        }

        var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
        Console.WriteLine($"Received event type: {eventType}");

        if (eventType == Event.Test.Name)
        {
            var testEvent = JsonSerializer.Deserialize<TestEvent>(consumeResult.Message.Value);
            if (testEvent != null)
            {
                Console.WriteLine($"Consumer: {groupInstanceName} Processing Test event: {testEvent}" +
                                  $" with Number: {testEvent.Number} with Offset: {consumeResult.Offset}");
            }
        }
        else
        {
            Console.WriteLine($"Unknown event type: {eventType}");
        }

        return Task.CompletedTask;
    }
}
