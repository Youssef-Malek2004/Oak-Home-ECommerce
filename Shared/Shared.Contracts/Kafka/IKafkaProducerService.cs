namespace Shared.Contracts.Kafka;

public interface IKafkaProducerService
{
    Task SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken, string eventType);
    Task SendMessageAsyncWithKey<T>(string topic, T message, CancellationToken cancellationToken, string eventType);
}