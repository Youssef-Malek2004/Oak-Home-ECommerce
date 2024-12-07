using Abstractions.ResultsPattern;
using Inventory.Domain.DTOs.InventoryDtos;
using MediatR;

namespace Inventory.Application.CQRS.Commands;
public record ChangeWarehouseCommand(ChangeWarehouseDto ChangeWarehouseDto) : IRequest<Result>;