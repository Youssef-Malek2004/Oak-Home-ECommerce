using System.Text.Json;
using Confluent.Kafka;
using Inventory.Application.KafkaSettings;
using Microsoft.Extensions.Options;

namespace Inventory.Infrastructure.Kafka;

public class KafkaProducerService
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(IOptions<KafkaSettings> settings)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers,
            AllowAutoCreateTopics = true,
            Acks = Acks.All
        };
        
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken)
    {
        var serializedMessage = JsonSerializer.Serialize(message);

        try
        {
            var result = await _producer.ProduceAsync(topic,
                new Message<string, string> { Key = typeof(T).Name, Value = serializedMessage }, cancellationToken);
            Console.WriteLine($"Message sent to topic {topic}: {result.Value} Offset: {result.Offset}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Error producing message: {ex.Error.Reason}");
        }

        _producer.Flush(cancellationToken);
    }
}