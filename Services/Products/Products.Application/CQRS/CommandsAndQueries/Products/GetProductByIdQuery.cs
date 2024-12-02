using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record GetProductByIdQuery(string Id) : IRequest<Result<Product>>;
