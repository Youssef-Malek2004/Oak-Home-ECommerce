using Shared.Contracts.Events;

namespace Products.Infrastructure.Kafka;

public class KafkaDispatcher(KafkaConsumerService consumer)
{
    public async Task StartConsuming(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            
            // consumer.StartConsuming<TestEvent>("testing-events","testing",
            //     async message =>
            // {
            //     Console.WriteLine($"Test received: {message.Name} @ Products Consumer");
            //     // Handle product creation logic
            //     await Task.CompletedTask;
            // }, stoppingToken);
            
        }, stoppingToken);
    }
}