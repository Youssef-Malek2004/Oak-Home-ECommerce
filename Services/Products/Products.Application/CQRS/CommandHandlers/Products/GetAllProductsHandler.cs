using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class GetAllProductsHandler(IProductsRepository repository)
    : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<Product>>>
{
    public async Task<Result<IEnumerable<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetProducts();
    }
}
