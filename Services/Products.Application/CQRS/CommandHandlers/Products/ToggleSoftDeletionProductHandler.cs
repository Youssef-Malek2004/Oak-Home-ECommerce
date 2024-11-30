using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class ToggleSoftDeletionProductHandler(IProductsRepository repository) : IRequestHandler<ToggleSoftDeletionProductCommand, Result>
{
    public async Task<Result> Handle(ToggleSoftDeletionProductCommand request, CancellationToken cancellationToken)
    {
        return await repository.ToggleSoftDeleteProduct(request.Id);
    }
}