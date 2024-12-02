using Shared.Contracts.Events;

namespace Products.Infrastructure.Kafka;

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
            _consumer.StartConsuming<TestEvent>("testing-events", async message =>
            {
                Console.WriteLine($"Test received: {message.Name} @ Products Consumer");
                // Handle product creation logic
                await Task.CompletedTask;
            }, stoppingToken);
        }, stoppingToken);
    }
}