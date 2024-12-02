using System.Text.Json;
using Confluent.Kafka;
using Inventory.Application.KafkaSettings;
using Inventory.Application.Services;
using Microsoft.Extensions.Options;

namespace Inventory.Infrastructure.Kafka;

public class KafkaConsumerService(IOptions<KafkaSettings> settings) : IKafkaConsumerService
{
    private const string InitialGroupInstanceId = "inventory-service-instance";
    public void StartConsuming<T>(string topic ,string groupInstanceName,
        Func<T, Task> processMessage, CancellationToken stoppingToken)
    {
        ConsumerConfig config = new()
        {
            BootstrapServers = settings.Value.BootstrapServers,
            GroupId = settings.Value.GroupId,
            AllowAutoCreateTopics = true,
            EnableAutoCommit = true,
            GroupInstanceId = InitialGroupInstanceId + groupInstanceName, //Works Fine without it now :)
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        
        consumer.Subscribe(topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);

                if (consumeResult is null)
                {
                    continue;
                }
                
                var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);

                if (message != null)
                {
                    Console.WriteLine($"Message received: {topic} Offset: {consumeResult.Offset}");
                    processMessage(message).Wait(stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Kafka consumer: {consumer.MemberId} operation canceled.");
        }
        finally
        {
            try
            {
                consumer.Close(); 
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Kafka consumer already disposed.");
            }
        }
    }
}