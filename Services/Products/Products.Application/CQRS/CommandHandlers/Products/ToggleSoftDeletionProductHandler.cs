using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Application.Services.Kafka;
using Products.Domain.Errors;
using Shared.Contracts.Events;
using Shared.Contracts.Events.ProductEvents;
using Shared.Contracts.Topics;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class ToggleSoftDeletionProductHandler(IProductsRepository repository, IKafkaProducerService kafkaProducerService) : IRequestHandler<ToggleSoftDeletionProductCommand, Result>
{
    public async Task<Result> Handle(ToggleSoftDeletionProductCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.ToggleSoftDeleteProduct(request.Id);

        if (result.IsFailure) return Result.Failure(ProductErrors.ProductUpdateFailedDeleted(request.Id));

        var isDeleted = result.Value;

        await kafkaProducerService.SendMessageAsync(Topics.ProductEvents.Name, new ProductSoftDeleted
        {
            ProductId = request.Id,
            IsDeleted = isDeleted
        }, cancellationToken, Event.ProductSoftDeleted.Name);

        return Result<bool>.Success(isDeleted);
    }
}