using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Products.Application.KafkaSettings;
using Products.Application.Services.Kafka;

namespace Products.Infrastructure.Kafka;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(IOptions<KafkaSettings> settings)
    {
        var kafkaConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");
        
        var config = new ProducerConfig
        {
            BootstrapServers = kafkaConnectionString,
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