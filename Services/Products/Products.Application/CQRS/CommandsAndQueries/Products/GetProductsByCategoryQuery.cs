using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record GetProductsByCategoryQuery(string CategoryId) : IRequest<Result<IEnumerable<Product>>>;
