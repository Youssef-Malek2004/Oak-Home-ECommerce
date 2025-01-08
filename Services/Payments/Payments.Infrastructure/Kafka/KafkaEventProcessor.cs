using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.CQRS.Commands;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Events.InventoryEvents;
using Shared.Contracts.Events.OrderEvents;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Payments.Infrastructure.Kafka;

public class KafkaEventProcessor(IServiceScopeFactory serviceScope, IKafkaProducerService kafkaProducerService)
{
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
    
    public async Task ProcessInventoryEvent(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
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

        if (eventType == Event.InventoryReserved.Name)
        {
            var inventoryReserved = JsonSerializer.Deserialize<InventoryReservedEvent>(consumeResult.Message.Value);
            if (inventoryReserved != null)
            {
                Console.WriteLine($"Processing InventoryReserved event: {inventoryReserved}");
                
                var result = await mediator.Send(new CheckPaymentCommand(inventoryReserved), cancellationToken);
                if (result.IsFailure)
                {
                    var notification = NotificationsFactory.
                        GenerateErrorWebNotificationUser(
                            inventoryReserved.UserId,
                            Groups.Users.Name,
                            result.Error.Code,
                            result.Error.Description!
                        );
                    
                    await kafkaProducerService.SendMessageAsync
                    (Topics.NotificationRequests.Name, new NotificationRequest
                    {
                        Notification = notification
                    }, cancellationToken, "");
                }
                else
                {
                    var notification = NotificationsFactory.
                        GenerateSuccessWebNotificationUser(
                            inventoryReserved.UserId,
                            Groups.Users.Name,
                            "Successful Payment",
                            $"Payment successful for order:{inventoryReserved.OrderId}"
                        );
                    
                    await kafkaProducerService.SendMessageAsync
                    (Topics.NotificationRequests.Name, new NotificationRequest
                    {
                        Notification = notification
                    }, cancellationToken, "");
                }
            }
        }
        else
        {
            Console.WriteLine($"Unknown event type: {eventType}");
        }
    }
}
