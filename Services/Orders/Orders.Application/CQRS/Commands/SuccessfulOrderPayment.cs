using Abstractions.ResultsPattern;
using MediatR;
using Shared.Contracts.Events.PaymentEvents;

namespace Orders.Application.CQRS.Commands;

public record SuccessfulOrderPayment(PaymentProcessedEvent PaymentProcessedEvent) : IRequest<Result>;