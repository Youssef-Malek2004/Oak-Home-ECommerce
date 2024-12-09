using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Application.Services.SignalR;
using Notifications.Domain.Entities;
using Shared.Contracts.Events;

namespace Notifications.Infrastructure.Kafka;

public class KafkaEventProcessor(IServiceScopeFactory serviceScope)
{
    public async Task ProcessTestEvent(ConsumeResult<string, string> consumeResult, string groupInstanceName, CancellationToken cancellationToken)
    {
        using var scope = serviceScope.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        
        var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

        if (eventTypeHeader == null)
        {
            Console.WriteLine("No eventType header found.");
            return;
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

                var notification = new Notification
                {
                    Title = "Test Event Notification",
                    Message = $"Test event received with number {testEvent.Number}",
                    Type = "info",
                    UserId = null, // Assuming TestEvent includes UserId todo broadcasting
                    Group = "TestEvents",
                    Channel = "WebSocket",
                    CreatedAt = DateTime.UtcNow,
                    IsDelivered = true,
                    IsRead = false
                };

                try
                {
                    if (notification.UserId != null)
                    {
                        await notificationService.SendNotificationToUserAsync(notification.UserId.Value, notification);
                    }
                    else
                    {
                        await notificationService.BroadcastNotificationAsync(notification);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending notification via SignalR: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Unknown event type: {eventType}");
        }
    }
}
