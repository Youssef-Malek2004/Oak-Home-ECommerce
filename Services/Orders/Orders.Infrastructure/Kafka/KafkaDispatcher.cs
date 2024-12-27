using Confluent.Kafka;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Orders.Infrastructure.Kafka;

public class KafkaDispatcher(IKafkaConsumerService consumer, KafkaEventProcessor eventProcessor) : IKafkaDispatcher
{
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
    public async Task StartConsumingPaymentEvents(CancellationToken stoppingToken, int instanceNumber)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ConsumeResult<string, string>>(
                Topics.PaymentEvents.Name,
                $"payments-events-consumer-{instanceNumber}",
                async consumeResult =>
                {
                    await eventProcessor.ProcessPaymentEvents(consumeResult, stoppingToken);
                },
                stoppingToken);
        }, stoppingToken);
    }
}