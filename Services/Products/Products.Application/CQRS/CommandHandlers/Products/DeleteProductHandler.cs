using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Shared.Contracts.Entities.NotificationService;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class DeleteProductHandler(IProductsRepository repository, IKafkaNotificationService kafkaNotificationService) : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteProduct(request.Id);
        
        if (result.IsFailure)
        {
            var failureNotification = NotificationsFactory.GenerateErrorWebNotificationUser(request.VendorId,
                Groups.Vendors.Name, "Product Deletion",
                $"Product: {request.Id} Deletion Failed!");

            await kafkaNotificationService.SendNotificationAsync(failureNotification, cancellationToken);
            
            return Result.Failure(result.Error);
        }
        
        var notification = NotificationsFactory.GenerateSuccessWebNotificationUser(request.VendorId,
            Groups.Vendors.Name, "Product Deletion", $"Product: {request.Id} deleted Successfully!");

        await kafkaNotificationService.SendNotificationAsync(notification, cancellationToken);

        return result;

    }
}
