using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Categories;

public record SearchCategoriesQuery(string SearchTerm) : IRequest<Result<IEnumerable<Category>>>;
