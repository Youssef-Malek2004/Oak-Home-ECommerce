using Abstractions.ResultsPattern;
using MediatR;
using Notifications.Domain.Entities;

namespace Notifications.Application.CQRS.Queries;

public record GetUnreadNotificationsQuery(Guid UserId) : IRequest<Result<List<Notification>>>;