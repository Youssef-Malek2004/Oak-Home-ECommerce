namespace Inventory.Application.Services;

public interface IKafkaProducerService
{
    Task SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken, string eventType);
    Task SendMessageAsyncWithKey<T>(string topic, T message, CancellationToken cancellationToken, string eventType);
}