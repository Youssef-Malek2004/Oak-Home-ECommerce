using Abstractions.ResultsPattern;
using MediatR;

namespace Products.Application.CQRS.CommandsAndQueries.Categories;

public record DeleteCategoryCommand(string Id) : IRequest<Result<bool>>;
