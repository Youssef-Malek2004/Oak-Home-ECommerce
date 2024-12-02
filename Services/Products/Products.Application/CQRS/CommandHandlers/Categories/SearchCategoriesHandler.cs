using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Categories;

public class SearchCategoriesHandler(ICategoryRepository repository)
    : IRequestHandler<SearchCategoriesQuery, Result<IEnumerable<Category>>>
{
    public async Task<Result<IEnumerable<Category>>> Handle(SearchCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await repository.SearchCategories(request.SearchTerm);
    }
}
