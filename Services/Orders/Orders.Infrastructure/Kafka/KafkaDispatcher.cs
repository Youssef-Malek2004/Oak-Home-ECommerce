using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.CQRS.Commands;
using Shared.Contracts.Events;
using Shared.Contracts.Events.InventoryEvents;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Orders.Infrastructure.Kafka;

public class KafkaDispatcher(IKafkaConsumerService consumer, IServiceScopeFactory serviceScope) : IKafkaDispatcher
{
   public async Task StartConsumingInventoryEvents(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(Topics.InventoryEvents.Name, "inventory-events-consumer-1", async consumeResult =>
            {
                using var scope = serviceScope.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

                if (eventTypeHeader != null)
                {
                    var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
                    Console.WriteLine($"Received event type: {eventType}");

                    if (eventType == Event.InventoryNotEnough.Name)
                    {
                        var inventoryNotEnough = JsonSerializer.Deserialize<NotEnoughInventoryEvent>(consumeResult.Message.Value);
                        if (inventoryNotEnough != null)
                        {
                            Console.WriteLine($"Processing inventoryNotEnough event: {inventoryNotEnough}");
                            await mediator.Send(new FailedOrderCommand(inventoryNotEnough), stoppingToken);
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