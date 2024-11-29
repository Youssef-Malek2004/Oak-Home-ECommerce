using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record GetAllProductsQuery() : IRequest<Result<IEnumerable<Product>>>;
