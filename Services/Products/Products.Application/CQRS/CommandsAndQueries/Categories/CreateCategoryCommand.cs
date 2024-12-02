using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.DTOs.CategoryDtos;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Categories;

public record CreateCategoryCommand(CreateCategoryDto Category) : IRequest<Result<Category>>;
