using Confluent.Kafka;

namespace Orders.Application.Services.Kafka;

public interface IKafkaConsumerService
{
    void StartConsuming<T>(string topic,string groupInstanceName, Func<ConsumeResult<string,string>, Task> processMessage, CancellationToken stoppingToken);
}