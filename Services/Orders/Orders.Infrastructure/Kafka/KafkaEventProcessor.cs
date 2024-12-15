using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.CQRS.Commands;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Events.InventoryEvents;
using Shared.Contracts.Events.OrderEvents;
using Shared.Contracts.Events.ProductEvents;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Orders.Infrastructure.Kafka;

public class KafkaEventProcessor(IServiceScopeFactory serviceScope, IKafkaProducerService kafkaProducerService)
{
    public async Task ProcessInventoryEvents(ConsumeResult<string, string> consumeResult,
        CancellationToken cancellationToken)
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
        
        if (eventType == Event.InventoryNotEnough.Name)
        {
            var inventoryNotEnough = JsonSerializer.Deserialize<NotEnoughInventoryEvent>(consumeResult.Message.Value);
            if (inventoryNotEnough != null)
            {
                Console.WriteLine($"Processing inventoryNotEnough event: {inventoryNotEnough}");
                await mediator.Send(new FailedOrderCommand(inventoryNotEnough), cancellationToken);
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
