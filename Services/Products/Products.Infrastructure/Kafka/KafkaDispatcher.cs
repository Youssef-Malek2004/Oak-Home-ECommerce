using Confluent.Kafka;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Products.Infrastructure.Kafka;

public class KafkaDispatcher(IKafkaConsumerService consumer, KafkaEventProcessor eventProcessor) : IKafkaDispatcher
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
    public async Task StartConsumingProductEvents(CancellationToken stoppingToken, int instanceNumber)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(
                Topics.ProductEvents.Name,
                $"product-events-consumer-{instanceNumber}",
                async consumeResult =>
                {
                    await eventProcessor.ProcessProductEvent(consumeResult, stoppingToken);
                },
                stoppingToken);
        }, stoppingToken);
    }
    
}