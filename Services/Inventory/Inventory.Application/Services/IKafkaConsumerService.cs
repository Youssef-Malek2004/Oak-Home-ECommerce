namespace Inventory.Application.Services;

public interface IKafkaConsumerService
{
    void StartConsuming<T>(string topic, Func<T, Task> processMessage, CancellationToken stoppingToken);
}