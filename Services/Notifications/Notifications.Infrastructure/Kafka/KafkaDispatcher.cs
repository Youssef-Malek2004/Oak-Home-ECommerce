using Confluent.Kafka;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Notifications.Infrastructure.Kafka;

public class KafkaDispatcher(IKafkaConsumerService consumer, KafkaEventProcessor eventProcessor)
{
    
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
