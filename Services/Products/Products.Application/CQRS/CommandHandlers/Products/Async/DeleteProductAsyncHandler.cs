using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products.Async;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Products.Application.CQRS.CommandHandlers.Products.Async;

public class DeleteProductAsyncHandler(IKafkaProducerService kafkaProducerService,
    IKafkaNotificationService kafkaNotificationService)
    : IRequestHandler<DeleteProductAsyncCommand, Result>
{
    public async Task<Result> Handle(DeleteProductAsyncCommand request, CancellationToken cancellationToken)
    {
        await kafkaProducerService.SendMessageAsync(Topics.ProductEvents.Name, request, cancellationToken,
            Event.ProductDeleteRequest.Name);

        var notification = NotificationsFactory.
            GenerateInfoWebNotificationUser(request.VendorId, Groups.Vendors.Name,
                "Product Deletion", $"Product: {request.ProductId} deletion in progress.");

        await kafkaNotificationService.SendNotificationAsync(notification, cancellationToken);
        
        return Result.Success();
    }
}