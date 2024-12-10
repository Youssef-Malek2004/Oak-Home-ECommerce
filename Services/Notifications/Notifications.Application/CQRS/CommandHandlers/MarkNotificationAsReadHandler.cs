using Abstractions.ResultsPattern;
using MediatR;
using Notifications.Application.CQRS.Commands;
using Notifications.Application.Services.Redis;

namespace Notifications.Application.CQRS.CommandHandlers;

public class MarkNotificationAsReadHandler(IRedisService redisService)
    : IRequestHandler<MarkNotificationAsReadCommand, Result>
{
    public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        return await redisService.MarkNotificationAsReadAsync(request.UserId,request.Group, request.NotificationId);
    }
}