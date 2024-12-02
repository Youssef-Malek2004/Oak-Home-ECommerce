
using Microsoft.Extensions.Hosting;

namespace Inventory.Infrastructure.Kafka;

public class KafkaHostedService : BackgroundService
{
    private readonly KafkaDispatcher _dispatcher;

    public KafkaHostedService(KafkaDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    protected override async  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield(); // Ensures non-blocking startup
        await _dispatcher.StartConsuming(stoppingToken);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping Kafka consumer...");
        await base.StopAsync(cancellationToken);
    }

    // public Task StartAsync(CancellationToken cancellationToken)
    // {
    //     Task.Run(() => _dispatcher.StartConsuming(), cancellationToken);
    //     return Task.CompletedTask;
    // }
    //
    // public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}