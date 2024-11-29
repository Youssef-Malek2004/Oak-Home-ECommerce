using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Application.Services;

namespace Products.Application.CQRS.CommandHandlers.Categories;

public class CategoryExistsHandler(ICategoryRepository repository) : IRequestHandler<CategoryExistsQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(CategoryExistsQuery request, CancellationToken cancellationToken)
    {
        return await repository.CategoryExists(request.Id);
    }
}
