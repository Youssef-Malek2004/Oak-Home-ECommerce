using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Categories;

public class GetAllCategoriesHandler(ICategoryRepository repository)
    : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<Category>>>
{
    public async Task<Result<IEnumerable<Category>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetCategories();
    }
}
