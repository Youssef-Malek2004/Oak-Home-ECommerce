using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Application.Services;

namespace Products.Application.CQRS.CommandHandlers.Categories;

public class DeleteCategoryHandler(ICategoryRepository repository)
    : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await repository.DeleteCategory(request.Id);
    }
}
