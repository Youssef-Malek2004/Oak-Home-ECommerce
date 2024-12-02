using Abstractions.ResultsPattern;
using MediatR;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record ToggleSoftDeletionProductCommand(string Id) : IRequest<Result>;
