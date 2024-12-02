namespace Inventory.Application.Services;

public interface IKafkaConsumerService
{
    void StartConsuming<T>(string topic,string groupInstanceName, Func<T, Task> processMessage, CancellationToken stoppingToken);
}