using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;

namespace Cart.Infrastructure.Kafka;

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
    
}
