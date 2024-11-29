using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.DTOs;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record UpdateProductCommand(string Id, UpdateProductDto Product) : IRequest<Result<Product>>;
