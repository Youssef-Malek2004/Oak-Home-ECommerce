using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Categories;

public record GetAllCategoriesQuery() : IRequest<Result<IEnumerable<Category>>>;
