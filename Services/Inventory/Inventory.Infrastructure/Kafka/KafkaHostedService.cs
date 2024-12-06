
using Microsoft.Extensions.Hosting;

namespace Inventory.Infrastructure.Kafka;

public class KafkaHostedService(KafkaDispatcher dispatcher) : BackgroundService
{
    protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        var consumingTasks = new List<Task>
        {
            // Task.Run(() => dispatcher.StartConsumingProductCreated(stoppingToken), stoppingToken),
            // Task.Run(() => dispatcher.StartConsumingProductSoftDeleted(stoppingToken), stoppingToken)
            Task.Run(() => dispatcher.StartConsumingProductEvents(stoppingToken), stoppingToken),
            Task.Run(() => dispatcher.StartConsumingOrderEvents(stoppingToken), stoppingToken)
        };

        await Task.WhenAll(consumingTasks);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping Kafka consumers...");
        await base.StopAsync(cancellationToken);
    }
}