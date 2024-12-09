using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.Entities;
using Products.Domain.Errors;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.RequestDtosAndMappers.SharedMappers.ProductMappers;
using Shared.Contracts.Topics;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class CreateProductHandler(IProductsRepository repository, IKafkaProducerService kafkaProducerService)
    : IRequestHandler<CreateProductCommand, Result<Product>>
{
    public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.CreateProduct(request.CreateProductDto, request.DynamicFields);

        if (result.IsFailure) return Result<Product>.Failure(result.Error);

        var product = result.Value;
        
        if (product is null) return Result<Product>.Failure(
            ProductErrors.ProductAddFailed("Product is null in the Product Create Handler"));
        
        var productCreatedEvent = ProductMappersShared.MapToProductCreated(
            product.ID, 
            request.CreateProductDto, 
            request.AddProductInventoryFields
        );

        await kafkaProducerService.SendMessageAsync(
            Topics.ProductEvents.Name, productCreatedEvent, cancellationToken, Event.ProductCreated.Name);
        
        return Result<Product>.Success(product);
        
    }
}
