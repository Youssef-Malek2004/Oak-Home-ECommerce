using Abstractions.ResultsPattern;
using MediatR;
using Shared.Contracts.Events.InventoryEvents;

namespace Orders.Application.CQRS.Commands;

public record FailedOrderCommand(NotEnoughInventoryEvent NotEnoughInventoryEvent) : IRequest<Result>;