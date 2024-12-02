using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Categories;

public class UpdateCategoryHandler(ICategoryRepository repository)
    : IRequestHandler<UpdateCategoryCommand, Result<Category>>
{
    public async Task<Result<Category>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await repository.UpdateCategory(request.Id, request.Category);
    }
}
