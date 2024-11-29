using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class SearchProductsHandler(IProductsRepository repository)
    : IRequestHandler<SearchProductsQuery, Result<IEnumerable<Product>>>
{
    public async Task<Result<IEnumerable<Product>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        return await repository.SearchProducts(request.SearchTerm);
    }
}
