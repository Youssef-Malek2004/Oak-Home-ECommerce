using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.CQRS.CommandsAndQueries.Products.Async;
using Products.Domain.DTOs.ProductDtos;
using Shared.Contracts.Events;

namespace Products.Infrastructure.Kafka;

public class KafkaEventProcessor(IServiceScopeFactory serviceScope)
{
    public async Task ProcessProductEvent(ConsumeResult<string, string> consumeResult,
        CancellationToken cancellationToken)
    {
        using var scope = serviceScope.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

        if (eventTypeHeader == null)
        {
            Console.WriteLine("No eventType header found.");
            return;
        }

        var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
        Console.WriteLine($"Received event type: {eventType}");
        
        if (eventType == Event.ProductCreateRequest.Name)
        {
            var request = JsonSerializer.Deserialize<CreateProductRequest>(consumeResult.Message.Value);
            await mediator.Send(new CreateProductCommand(request!.AddProductInventoryFields, request.CreateProductDto, request.DynamicFields), cancellationToken);   
        }
        else if(eventType == Event.ProductDeleteRequest.Name)
        {
            var request = JsonSerializer.Deserialize<DeleteProductAsyncCommand>(consumeResult.Message.Value);
            await mediator.Send(new DeleteProductCommand(request!.ProductId, request.VendorId), cancellationToken);   
        }
        else if(eventType == Event.ProductUpdateRequest.Name)
        {
            var request = JsonSerializer.Deserialize<UpdateProductAsyncCommand>(consumeResult.Message.Value);
            await mediator.Send(new UpdateProductCommand(request!.Id,request.Product, request.DynamicFields),
                cancellationToken);   
        }
        else
        {
            Console.WriteLine($"Unknown event type: {eventType}");
        }
    }
    
    public Task ProcessTestEvent(ConsumeResult<string, string> consumeResult,
        string groupInstanceName, CancellationToken cancellationToken)
    {

        var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "eventType");

        if (eventTypeHeader == null)
        {
            Console.WriteLine("No eventType header found.");
            return Task.CompletedTask;
        }

        var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
        Console.WriteLine($"Received event type: {eventType}");

        if (eventType == Event.Test.Name)
        {
            var testEvent = JsonSerializer.Deserialize<TestEvent>(consumeResult.Message.Value);
            if (testEvent != null)
            {
                Console.WriteLine($"Consumer: {groupInstanceName} Processing Test event: {testEvent}" +
                                  $" with Number: {testEvent.Number} with Offset: {consumeResult.Offset}");
            }
        }
        else
        {
            Console.WriteLine($"Unknown event type: {eventType}");
        }

        return Task.CompletedTask;
    }
}