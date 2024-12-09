using Abstractions.ResultsPattern;
using MediatR;
using Notifications.Application.CQRS.Queries;
using Notifications.Application.Services.Redis;
using Shared.Contracts.Entities;
using Shared.Contracts.Entities.NotificationService;

namespace Notifications.Application.CQRS.QueryHandlers;

public class GetUnreadNotificationsHandler(IRedisService redisService)
    : IRequestHandler<GetUnreadNotificationsQuery, Result<List<Notification>>>
{
    public async Task<Result<List<Notification>>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await redisService.GetUnreadNotificationsAsync(request.UserId);
    }
}