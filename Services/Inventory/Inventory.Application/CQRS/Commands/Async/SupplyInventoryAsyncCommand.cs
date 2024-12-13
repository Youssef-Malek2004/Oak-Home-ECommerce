using Abstractions.ResultsPattern;
using Inventory.Domain.DTOs.InventoryDtos;
using MediatR;

namespace Inventory.Application.CQRS.Commands.Async;

public record SupplyInventoryAsyncCommand(Guid VendorId,SupplyInventoryDto SupplyInventoryDto) : IRequest<Result>;