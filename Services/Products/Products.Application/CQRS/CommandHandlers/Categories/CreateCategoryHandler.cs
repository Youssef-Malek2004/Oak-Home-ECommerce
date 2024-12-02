using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Categories;

public class CreateCategoryHandler(ICategoryRepository repository)
    : IRequestHandler<CreateCategoryCommand, Result<Category>>
{
    public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await repository.CreateCategory(request.Category);
    }
}
