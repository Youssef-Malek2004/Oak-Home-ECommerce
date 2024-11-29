using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class GetProductsByCategoryHandler(IProductsRepository repository)
    : IRequestHandler<GetProductsByCategoryQuery, Result<IEnumerable<Product>>>
{
    public async Task<Result<IEnumerable<Product>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetProductsByCategory(request.CategoryId);
    }
}