using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.DTOs;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;
using Shared.Contracts.RequestDtosAndMappers.ProductDtos;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record CreateProductCommand(AddProductInventoryFields AddProductInventoryFields, CreateProductDto CreateProductDto, IDictionary<string, object>? DynamicFields) 
    : IRequest<Result<Product>>;
