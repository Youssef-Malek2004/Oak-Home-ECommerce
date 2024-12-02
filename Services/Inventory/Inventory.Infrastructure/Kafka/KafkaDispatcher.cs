using Inventory.Application.CQRS.Events;
using Inventory.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Events.ProductEvents;
using Shared.Contracts.Topics;

namespace Inventory.Infrastructure.Kafka;

public class KafkaDispatcher(IKafkaConsumerService consumer, IServiceScopeFactory serviceScope)
{
    public async Task StartConsumingProductCreated(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            consumer.StartConsuming<ProductCreated>(Topics.ProductCreatedTopic.Name, async message =>
            {
                using var scope = serviceScope.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                await mediator.Send(new ProductCreatedEvent(message), stoppingToken); 
            }, stoppingToken);
            
        }, stoppingToken);
        
    }
}