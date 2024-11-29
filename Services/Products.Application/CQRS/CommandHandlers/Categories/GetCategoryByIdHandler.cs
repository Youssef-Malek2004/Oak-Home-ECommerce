using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Categories;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Categories;

public class GetCategoryByIdHandler(ICategoryRepository repository)
    : IRequestHandler<GetCategoryByIdQuery, Result<Category>>
{
    public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetCategoryById(request.Id);
    }
}
