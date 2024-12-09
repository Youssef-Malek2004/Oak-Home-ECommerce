using Microsoft.Extensions.Hosting;

namespace Notifications.Infrastructure.Kafka;

public class KafkaHostedService(KafkaDispatcher dispatcher) : BackgroundService
{
    protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        var consumingTasks = new List<Task>
        {
            // Task.Run(() => dispatcher.StartConsumingTestEvents(stoppingToken,1), stoppingToken),
            // Task.Run(() => dispatcher.StartConsumingTestEvents(stoppingToken,2), stoppingToken),
            // Task.Run(() => dispatcher.StartConsumingTestEvents(stoppingToken,3), stoppingToken),
            
            Task.Run(() => dispatcher.StartConsumingNotificationRequests(stoppingToken,1), stoppingToken),
            // Task.Run(() => dispatcher.StartConsumingNotificationRequests(stoppingToken,2), stoppingToken),
            // Task.Run(() => dispatcher.StartConsumingNotificationRequests(stoppingToken,3), stoppingToken),
        };

        await Task.WhenAll(consumingTasks);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping Kafka consumers...");
        await base.StopAsync(cancellationToken);
    }
}