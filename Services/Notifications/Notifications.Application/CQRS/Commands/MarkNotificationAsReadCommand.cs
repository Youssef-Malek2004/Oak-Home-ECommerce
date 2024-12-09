using Abstractions.ResultsPattern;
using MediatR;

namespace Notifications.Application.CQRS.Commands;

public record MarkNotificationAsReadCommand(Guid UserId, Guid NotificationId) : IRequest<Result>;