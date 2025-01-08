using Microsoft.Extensions.Hosting;

namespace Orders.Infrastructure.Kafka;

public class KafkaHostedService(KafkaDispatcher dispatcher) : BackgroundService
{
    protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        var consumingTasks = new List<Task>
        {
            Task.Run(() => dispatcher.StartConsumingInventoryEvents(stoppingToken,1), stoppingToken),
            Task.Run(() => dispatcher.StartConsumingPaymentEvents(stoppingToken,1), stoppingToken)
        };

        await Task.WhenAll(consumingTasks);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping Kafka consumers...");
        await base.StopAsync(cancellationToken);
    }
}