using Microsoft.Extensions.Hosting;

namespace Products.Infrastructure.Kafka;

public class KafkaHostedService(KafkaDispatcher dispatcher) : BackgroundService
{
    protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        await dispatcher.StartConsuming(stoppingToken);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping Kafka consumers...");
        await base.StopAsync(cancellationToken);
    }
}