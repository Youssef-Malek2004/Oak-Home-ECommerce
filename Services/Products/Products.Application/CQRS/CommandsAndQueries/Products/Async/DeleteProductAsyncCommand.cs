using Abstractions.ResultsPattern;
using MediatR;

namespace Products.Application.CQRS.CommandsAndQueries.Products.Async;

public record DeleteProductAsyncCommand(string ProductId, string VendorId) : IRequest<Result>;