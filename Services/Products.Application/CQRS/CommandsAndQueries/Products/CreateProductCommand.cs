using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.DTOs;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record CreateProductCommand(CreateProductDto CreateProductDto, IDictionary<string, object>? DynamicFields) 
    : IRequest<Result<Product>>;
