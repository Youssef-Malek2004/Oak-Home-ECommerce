using Abstractions.ResultsPattern;
using MediatR;
using Shared.Contracts.Events.ProductEvents;

namespace Inventory.Application.CQRS.Events;

public record ProductSoftDeletedEvent(ProductSoftDeleted ProductSoftDeleted) : IRequest<Result>;