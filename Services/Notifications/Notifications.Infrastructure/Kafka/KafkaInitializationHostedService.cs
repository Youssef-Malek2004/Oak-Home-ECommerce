using Microsoft.Extensions.Hosting;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Notifications.Infrastructure.Kafka;

public class KafkaInitializationHostedService(IAdminClientService adminClientService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Console.WriteLine("Starting Kafka Initialization...");

            var topic = Topics.TestingTopic.Name;

            await adminClientService.CreateTopicAsync(topic, 3, 1);
            
            var topicCount = await adminClientService.GetPartitionCountAsync(topic);
            
            Console.WriteLine($"Partition Count for Topic:{topic} is: {topicCount.Value}");

            Console.WriteLine("Kafka Initialization completed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during Kafka initialization: {ex.Message}");
        }
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Kafka Initialization Service Down");
        await base.StopAsync(cancellationToken);
    }
}