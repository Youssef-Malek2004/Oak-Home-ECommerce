using Confluent.Kafka;

namespace Shared.Contracts.Kafka;

public interface IKafkaConsumerService
{
    void StartConsuming<T>(string topic,string groupInstanceName, Func<ConsumeResult<string,string>, Task> processMessage, CancellationToken stoppingToken);
}