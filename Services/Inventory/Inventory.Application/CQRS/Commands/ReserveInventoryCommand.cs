using Abstractions.ResultsPattern;
using MediatR;
using Shared.Contracts.Events.OrderEvents;

namespace Inventory.Application.CQRS.Commands;

public record ReserveInventoryCommand(OrderCreatedEvent OrderCreatedEvent) : IRequest<Result>;