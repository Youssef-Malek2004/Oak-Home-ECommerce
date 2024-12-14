using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.Entities;
using Products.Domain.Errors;
using Shared.Contracts.Entities.NotificationService;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class UpdateProductHandler(IProductsRepository repository, IKafkaNotificationService kafkaNotificationService)
    : IRequestHandler<UpdateProductCommand, Result<Product>>
{
    public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var result =  await repository.UpdateProduct(request.Id, request.Product, request.DynamicFields);
        
        if (result.IsFailure)
        {
            var failureNotification = NotificationsFactory.
                GenerateErrorWebNotificationUser(request.Product.VendorId!,
                Groups.Vendors.Name, "Product Update",
                $"Product: {request.Product.Name} Update Failed!");

            await kafkaNotificationService.SendNotificationAsync(failureNotification, cancellationToken);
            
            return Result<Product>.Failure(result.Error);
        }
        
        var product = result.Value;
        
        if (product is null) return Result<Product>.Failure(
            ProductErrors.ProductEditFailed("Product is null in the Product Update Handler"));
        
        
        var notification = NotificationsFactory.GenerateSuccessWebNotificationUser(request.Product.VendorId!,
            Groups.Vendors.Name, "Product Update", 
            $"Product: {request.Product.Name} Updated Successfully!");

        await kafkaNotificationService.SendNotificationAsync(notification, cancellationToken);
        
        return Result<Product>.Success(product);
        
    }
}
