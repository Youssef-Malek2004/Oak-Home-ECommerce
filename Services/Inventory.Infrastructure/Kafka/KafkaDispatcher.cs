using Inventory.Application.Events;

namespace Inventory.Infrastructure.Kafka;

public class KafkaDispatcher
{
    private readonly KafkaConsumerService _consumer;

    public KafkaDispatcher(KafkaConsumerService consumer)
    {
        _consumer = consumer;
    }

    public async Task StartConsuming(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            _consumer.StartConsuming<Test>("testing-events", async message =>
            {
                Console.WriteLine($"Test received: {message.Name}");
                // Handle product creation logic
                await Task.CompletedTask;
            }, stoppingToken);
        }, stoppingToken);
        
        await Task.Run(() =>
        {
            _consumer.StartConsuming<Test2>("testing-events", async message =>
            {
                Console.WriteLine($"Test 2 received: {message.Name} {message.Num}");
                await Task.CompletedTask;
            }, stoppingToken);
        }, stoppingToken);
        
    }
}