using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products.Async;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Products.Application.CQRS.CommandHandlers.Products.Async;

public class CreateProductAsyncHandler(IKafkaProducerService kafkaProducerService,
    IKafkaNotificationService kafkaNotificationService)
    : IRequestHandler<CreateProductAsyncCommand, Result>
{
    public async Task<Result> Handle(CreateProductAsyncCommand request, CancellationToken cancellationToken)
    {
        await kafkaProducerService.SendMessageAsync(Topics.ProductEvents.Name, request, cancellationToken,
            Event.ProductCreateRequest.Name);

        var notification = NotificationsFactory.
            GenerateInfoWebNotificationUser(request.CreateProductDto.VendorId, Groups.Vendors.Name,
                "Product Creation", $"Product: {request.CreateProductDto.Name} creation in progress.");

        await kafkaNotificationService.SendNotificationAsync(notification, cancellationToken);
        
        return Result.Success();
    }
}