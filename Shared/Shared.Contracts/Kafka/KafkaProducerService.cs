using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Shared.Contracts.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(IOptions<KafkaSettings> settings)
    {
        var kafkaConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");
        
        var config = new ProducerConfig
        {
            BootstrapServers = kafkaConnectionString ?? settings.Value.BootstrapServers,
            AllowAutoCreateTopics = true,
            Acks = Acks.All
        };
        
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken, string eventType)
    {
        var serializedMessage = JsonSerializer.Serialize(message);

        try
        {
            var result = await _producer.ProduceAsync(topic,
                new Message<string, string> { 
                    Key= Guid.NewGuid().ToString(),
                    Value = serializedMessage, 
                    Headers = new Headers {
                    { "eventType", Encoding.UTF8.GetBytes(eventType) }
                    } 
                }, cancellationToken);
            Console.WriteLine($"Message sent to topic {topic}: {result.Value} Offset: {result.Offset}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Error producing message: {ex.Error.Reason}");
        }

        _producer.Flush(cancellationToken);
    }
    
    public async Task SendMessageAsyncWithKey<T>(string topic, T message, CancellationToken cancellationToken, string eventType)
    {
        var serializedMessage = JsonSerializer.Serialize(message);

        try
        {
            var result = await _producer.ProduceAsync(topic,
                new Message<string, string> { 
                    Key = typeof(T).Name, 
                    Value = serializedMessage, 
                    Headers = new Headers {
                        { "eventType", Encoding.UTF8.GetBytes(eventType) }
                    } 
                }, cancellationToken);
            Console.WriteLine($"Message sent to topic {topic}: {result.Value} Offset: {result.Offset}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Error producing message: {ex.Error.Reason}");
        }

        _producer.Flush(cancellationToken);
    }
}