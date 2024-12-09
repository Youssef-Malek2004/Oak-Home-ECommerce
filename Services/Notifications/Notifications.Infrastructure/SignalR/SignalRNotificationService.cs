using System.Text.Json;
using Abstractions.ResultsPattern;
using Microsoft.AspNetCore.SignalR;
using Notifications.Application.Services.Redis;
using Notifications.Application.Services.SignalR;
using Notifications.Domain.Entities;
using Notifications.Domain.Errors;

namespace Notifications.Infrastructure.SignalR;

public class SignalRNotificationService(IHubContext<ChatHub, IChatClient> hubContext,
    IRedisService redisService,
    IUserConnectionManager userConnectionManager) : INotificationService
{
    public async Task<Result> SendNotificationToUserAsync(Guid userId, Notification notification)
    {
        if (userConnectionManager.IsUserConnected(userId))
        {
            await hubContext.Clients.Group(userId.ToString()).ReceiveNotification(JsonSerializer.Serialize(notification));
            notification.IsDelivered = true;
        }

        await redisService.AddNotificationAsync(userId, notification);
        
        return Result.Success();
    }

    public async Task<Result> BroadcastNotificationAsync(Notification notification)
    {
        await hubContext.Clients.All.ReceiveNotification(JsonSerializer.Serialize(notification));
        return Result.Success();
    }
    
    public async Task<Result> SendUndeliveredNotificationsAsync(Guid userId)
    {
        try
        {
            var result = await redisService.GetUndeliveredNotificationsAsync(userId);

            if (result.IsFailure) return Result.Failure(result.Error);
            if (result.Value is null)
                return Result.Failure(NotificationErrors.FailedToGetUndeliveredNotifications(userId));

            var undeliveredNotifications = result.Value;

            foreach (var notification in undeliveredNotifications)
            {
                await hubContext.Clients.User(userId.ToString())
                    .ReceiveNotification(JsonSerializer.Serialize(notification));
                
                await redisService.MarkNotificationAsDeliveredAsync(userId, notification.Id);
            }
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToSendUndeliveredNotifications(userId));
        }

        return Result.Success();
    }
    
    public async Task<Result> SendUnreadNotificationsAsync(Guid userId)
    {
        try
        {
            var result = await redisService.GetUnreadNotificationsAsync(userId);

            if (result.IsFailure) return Result.Failure(result.Error);
            if (result.Value is null)
                return Result.Failure(NotificationErrors.FailedToGetUnreadNotifications(userId));

            var unreadNotifications = result.Value;

            foreach (var notification in unreadNotifications)
            {
                await hubContext.Clients.User(userId.ToString())
                    .ReceiveNotification(JsonSerializer.Serialize(notification));
            }
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToSendUndeliveredNotifications(userId));
        }

        return Result.Success();
    }
    
}