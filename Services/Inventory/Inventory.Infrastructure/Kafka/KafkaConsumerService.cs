using System.Text.Json;
using Confluent.Kafka;
using Inventory.Application.KafkaSettings;
using Inventory.Application.Services;
using Microsoft.Extensions.Options;

namespace Inventory.Infrastructure.Kafka;

public class KafkaConsumerService(IOptions<KafkaSettings> settings, IAdminClient adminClient) : IKafkaConsumerService
{
    private const string InitialGroupInstanceId = "inventory-service-instance";
    public void StartConsuming<T>(string topic ,string groupInstanceName,
        Func<T, Task> processMessage, CancellationToken stoppingToken)
    {
        var kafkaConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");
        
        ConsumerConfig config = new()
        {
            BootstrapServers = kafkaConnectionString,
            GroupId = settings.Value.GroupId,
            AllowAutoCreateTopics = true,
            EnableAutoCommit = true,
            GroupInstanceId = InitialGroupInstanceId + groupInstanceName, //Works Fine without it now :)
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        
        if (!metadata.Topics.Any(t => t.Topic == topic && t.Error.Code == ErrorCode.NoError))
        {
            Console.WriteLine($"Error: Topic '{topic}' does not exist.");
            return;
        }
        
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