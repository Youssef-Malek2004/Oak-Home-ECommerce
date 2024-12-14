using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products.Async;
using Products.Domain.Errors;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Products.Application.CQRS.CommandHandlers.Products.Async;

public class UpdateProductAsyncHandler(
    IKafkaProducerService kafkaProducerService,
    IKafkaNotificationService kafkaNotificationService) : IRequestHandler<UpdateProductAsyncCommand, Result>
{
    public async Task<Result> Handle(UpdateProductAsyncCommand request, CancellationToken cancellationToken)
    {
        await kafkaProducerService.SendMessageAsync(Topics.ProductEvents.Name, request, cancellationToken,
            Event.ProductUpdateRequest.Name);

        if (request.Product.VendorId == null)
            return Result.Failure(ProductErrors.InvalidProductData("Missing Vendor Id"));

        var notification = NotificationsFactory.GenerateInfoWebNotificationUser(request.Product.VendorId,
            Groups.Vendors.Name,
            "Product Update", $"Product: {request.Id} update in progress.");

        await kafkaNotificationService.SendNotificationAsync(notification, cancellationToken);

        return Result.Success();
    }
}