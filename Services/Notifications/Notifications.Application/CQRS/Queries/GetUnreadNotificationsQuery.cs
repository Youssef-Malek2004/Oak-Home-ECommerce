using Abstractions.ResultsPattern;
using MediatR;
using Shared.Contracts.Entities;
using Shared.Contracts.Entities.NotificationService;

namespace Notifications.Application.CQRS.Queries;

public record GetUnreadNotificationsQuery(Guid UserId) : IRequest<Result<List<Notification>>>;