using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.DTOs.ProductDtos;

namespace Products.Application.CQRS.CommandsAndQueries.Products.Async;
public record UpdateProductAsyncCommand(string Id, UpdateProductDto Product,
    IDictionary<string, object>? DynamicFields) : IRequest<Result>;