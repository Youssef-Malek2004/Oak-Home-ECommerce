using Abstractions.ResultsPattern;
using Inventory.Domain.DTOs.WarehouseDtos;
using MediatR;

namespace Inventory.Application.CQRS.Queries;

public record GetWarehouseForProductQuery(string ProductId) : IRequest<Result<WarehouseForProductDto>>;