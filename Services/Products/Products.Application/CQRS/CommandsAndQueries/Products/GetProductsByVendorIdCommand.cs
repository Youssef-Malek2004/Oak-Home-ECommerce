using Abstractions.ResultsPattern;
using MediatR;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandsAndQueries.Products;

public record GetProductsByVendorIdCommand(string VendorId) : IRequest<Result<IEnumerable<VendorGetProductsDto>>>;