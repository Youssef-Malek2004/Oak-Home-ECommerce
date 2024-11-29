using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class DeleteProductHandler(IProductsRepository repository) : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        return await repository.DeleteProduct(request.Id);
    }
}
