using System.Text.Json;
using Abstractions.ResultsPattern;
using Microsoft.AspNetCore.SignalR;
using Notifications.Application.Services.Redis;
using Notifications.Application.Services.SignalR;
using Notifications.Domain.Errors;
using Shared.Contracts.Entities.NotificationService;

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
    
    public async Task<Result> SendNotificationToGroup(Notification notification)
    {
        try
        {
            var redisResult = await redisService.AddNotificationToGroupAsync(notification);
            if (redisResult.IsFailure)
                return Result.Failure(redisResult.Error);
            
            await hubContext.Clients.Group(notification.Group).ReceiveNotification(JsonSerializer.Serialize(notification));

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(NotificationErrors.FailedToSendNotificationToGroup(notification.Group));
        }
    }
    
    public async Task<Result> SendUndeliveredNotificationsAsync(Guid userId, string group)
    {
        try
        {
            var result = await redisService.GetUndeliveredNotificationsAsync(userId, group);

            if (result.IsFailure) return Result.Failure(result.Error);
            if (result.Value is null)
                return Result.Failure(NotificationErrors.FailedToGetUndeliveredNotifications(userId));

            var undeliveredNotifications = result.Value;

            foreach (var notification in undeliveredNotifications)
            {
                await hubContext.Clients.User(userId.ToString())
                    .ReceiveNotification(JsonSerializer.Serialize(notification));
                
                await redisService.MarkNotificationAsDeliveredAsync(userId,group, notification.Id);
            }
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToSendUndeliveredNotifications(userId));
        }

        return Result.Success();
    }
    
    public async Task<Result> SendUnreadNotificationsAsync(Guid userId, string group)
    {
        try
        {
            var result = await redisService.GetUnreadNotificationsAsync(userId, group);

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