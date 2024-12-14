
using Microsoft.Extensions.Hosting;

namespace Inventory.Infrastructure.Kafka;

public class KafkaHostedService(KafkaDispatcher dispatcher) : BackgroundService
{
    protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        var consumingTasks = new List<Task>
        {
            Task.Run(() => dispatcher.StartConsumingProductEvents(stoppingToken,1), stoppingToken),
            Task.Run(() => dispatcher.StartConsumingOrderEvents(stoppingToken,1), stoppingToken),
            
            Task.Run(() => dispatcher.StartConsumingInventoryEvents(stoppingToken,1), stoppingToken),
            Task.Run(() => dispatcher.StartConsumingInventoryEvents(stoppingToken,2), stoppingToken),
            Task.Run(() => dispatcher.StartConsumingInventoryEvents(stoppingToken,3), stoppingToken),
            Task.Run(() => dispatcher.StartConsumingInventoryEvents(stoppingToken,4), stoppingToken),
            Task.Run(() => dispatcher.StartConsumingInventoryEvents(stoppingToken,5), stoppingToken),
            
            // Task.Run(() => dispatcher.StartConsumingTestEvents(stoppingToken,1), stoppingToken),
            // Task.Run(() => dispatcher.StartConsumingTestEvents(stoppingToken,2), stoppingToken),
            // Task.Run(() => dispatcher.StartConsumingTestEvents(stoppingToken,3), stoppingToken),
        };

        await Task.WhenAll(consumingTasks);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping Kafka consumers...");
        await base.StopAsync(cancellationToken);
    }
}