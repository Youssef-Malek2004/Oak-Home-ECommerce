using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.Entities;
using Shared.Contracts.RequestDtosAndMappers.ProductDtos;

namespace Products.Application.CQRS.CommandsAndQueries.Products.Async;

public record CreateProductAsyncCommand(AddProductInventoryFields AddProductInventoryFields, CreateProductDto CreateProductDto, IDictionary<string, object>? DynamicFields) 
    : IRequest<Result>;