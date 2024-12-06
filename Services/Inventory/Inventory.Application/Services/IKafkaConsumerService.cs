using Confluent.Kafka;

namespace Inventory.Application.Services;

public interface IKafkaConsumerService
{
    void StartConsuming<T>(string topic,string groupInstanceName, Func<ConsumeResult<string,string>, Task> processMessage, CancellationToken stoppingToken);
}