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

            var notificationRequestsTopic = Topics.NotificationRequests.Name;

            // await adminClientService.CreateTopicAsync(notificationRequestsTopic, 3, 1);

            await adminClientService.AddPartitionsAsync(notificationRequestsTopic, 9);
            
            var topicCount = await adminClientService.GetPartitionCountAsync(notificationRequestsTopic);
            
            Console.WriteLine($"Partition Count for Topic:{notificationRequestsTopic} is: {topicCount.Value}");

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