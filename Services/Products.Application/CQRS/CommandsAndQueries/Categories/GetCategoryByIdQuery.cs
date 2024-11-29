using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Categories;

public record GetCategoryByIdQuery(string Id) : IRequest<Result<Category>>;
