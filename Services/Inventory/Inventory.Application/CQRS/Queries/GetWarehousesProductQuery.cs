using Abstractions.ResultsPattern;
using Inventory.Domain.DTOs.WarehouseDtos;
using MediatR;

namespace Inventory.Application.CQRS.Queries;

public record GetWarehousesProductQuery : IRequest<Result<IEnumerable<WarehouseNamesDto>>>;
