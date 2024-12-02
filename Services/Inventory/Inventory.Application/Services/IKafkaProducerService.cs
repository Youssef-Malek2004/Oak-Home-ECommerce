namespace Inventory.Application.Services;

public interface IKafkaProducerService
{
    Task SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken);
}