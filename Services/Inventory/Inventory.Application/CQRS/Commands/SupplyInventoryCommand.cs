using Abstractions.ResultsPattern;
using Inventory.Domain.DTOs.InventoryDtos;
using MediatR;

namespace Inventory.Application.CQRS.Commands;

public record SupplyInventoryCommand(Guid VendorId,SupplyInventoryDto SupplyInventoryDto) : IRequest<Result>;