using Abstractions.ResultsPattern;
using Inventory.Domain.Entities;
using MediatR;
using Shared.Contracts.Events.ProductEvents;

namespace Inventory.Application.CQRS.Events;

public record ProductCreatedEvent(ProductCreated ProductCreated) : IRequest<Result<Inventories>>;