using Abstractions.ResultsPattern;
using MediatR;

namespace Products.Application.CQRS.CommandsAndQueries.Categories;

public record CategoryExistsQuery(string Id) : IRequest<Result<bool>>;
