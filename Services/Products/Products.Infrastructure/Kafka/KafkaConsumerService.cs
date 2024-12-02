using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Products.Application.KafkaSettings;

namespace Products.Infrastructure.Kafka;

public class KafkaConsumerService(IOptions<KafkaSettings> settings)
{
    private const string InitialGroupInstanceId = "products-service-instance";
    public void StartConsuming<T>(string topic,string groupInstanceName, Func<T, Task> processMessage, CancellationToken stoppingToken)
    {
        ConsumerConfig config = new()
        {
            BootstrapServers = settings.Value.BootstrapServers,
            GroupId = settings.Value.GroupId,
            AllowAutoCreateTopics = true,
            GroupInstanceId = InitialGroupInstanceId + groupInstanceName,
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
                    // Console.WriteLine($"Message received: {consumeResult.Message.Value} Offset: {consumeResult.Offset}");
                    processMessage(message).Wait(stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Kafka consumer: {consumer.Name} operation canceled.");
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