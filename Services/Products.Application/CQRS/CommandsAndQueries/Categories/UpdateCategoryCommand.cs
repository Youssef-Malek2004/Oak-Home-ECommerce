using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.DTOs.CategoryDtos;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Categories;

public record UpdateCategoryCommand(string Id, UpdateCategoryDto Category) : IRequest<Result<Category>>;
