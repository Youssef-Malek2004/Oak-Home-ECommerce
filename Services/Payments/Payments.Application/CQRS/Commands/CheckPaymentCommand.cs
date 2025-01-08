using Abstractions.ResultsPattern;
using MediatR;
using Shared.Contracts.Events.InventoryEvents;

namespace Payments.Application.CQRS.Commands;

public record CheckPaymentCommand(InventoryReservedEvent InventoryReservedEvent) : IRequest<Result>;