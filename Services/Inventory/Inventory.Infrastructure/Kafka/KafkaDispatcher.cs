using Confluent.Kafka;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Inventory.Infrastructure.Kafka;

public class KafkaDispatcher(IKafkaConsumerService consumer, KafkaEventProcessor eventProcessor) : IKafkaDispatcher
{
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
    public async Task StartConsumingInventoryEvents(CancellationToken stoppingToken, int instanceNumber)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(
                Topics.InventoryEvents.Name,
                $"inventory-events-consumer-{instanceNumber}",
                async consumeResult =>
                {
                    await eventProcessor.ProcessInventoryEvents(consumeResult, stoppingToken);
                },
                stoppingToken);
        }, stoppingToken);
    }

    public async Task StartConsumingOrderEvents(CancellationToken stoppingToken, int instanceNumber)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(
                Topics.OrderEvents.Name,
                $"order-events-consumer-{instanceNumber}",
                async consumeResult =>
                {
                    await eventProcessor.ProcessOrderEvent(consumeResult, stoppingToken);
                },
                stoppingToken);
        }, stoppingToken);
    }
    
    public async Task StartConsumingTestEvents(CancellationToken stoppingToken, int instanceNumber)
    {
        var groupInstanceName = $"test-events-consumer-{instanceNumber}";
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(
                Topics.TestingTopic.Name,
                groupInstanceName,
                async consumeResult =>
                {
                    await eventProcessor.ProcessTestEvent(consumeResult,groupInstanceName, stoppingToken);
                },
                stoppingToken);
        }, stoppingToken);
    }
}
