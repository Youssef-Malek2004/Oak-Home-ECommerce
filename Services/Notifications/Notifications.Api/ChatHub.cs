using Microsoft.AspNetCore.SignalR;
using Notifications.Application.Services.Redis;

namespace Notifications.Api;

public sealed class ChatHub(IRedisService redisService) : Hub<IChatClient>
{
    public async Task SendMessage(string message)
    {
        await Clients.All.ReceiveNotification($"{Context.ConnectionId}: {message}");
    }

    public override async Task OnConnectedAsync()
    {
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (Guid.TryParse(userIdString, out var userId))
        {
            
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
            
            var unreadResult = await redisService.GetUnreadNotificationsAsync(userId);

            if (unreadResult.IsSuccess && unreadResult.Value != null)
            {
                foreach (var notification in unreadResult.Value)
                {
                    await Task.Delay(1);
                    await Clients.Caller.ReceiveNotification($"Unread Notification: {notification.Title} - {notification.Message}");
                }
            }
            else
            {
                await Task.Delay(1);
                await Clients.Caller.ReceiveNotification($"Failed to retrieve unread notifications: {unreadResult.Error}");
            }
            await Clients.Caller.ReceiveNotification($"Connected as user {userId}.");
        }
        else
        {
            await Clients.Caller.ReceiveNotification("Connection failed: Valid UserId is required.");
            Context.Abort();
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task MarkNotificationAsRead(Guid notificationId)
    {
        var userIdString = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        if (!Guid.TryParse(userIdString, out var userId))
        {
            await Clients.Caller.ReceiveNotification("Error: Valid UserId is required.");
            return;
        }
        
        var result = await redisService.MarkNotificationAsReadAsync(userId, notificationId);

        if (result.IsSuccess)
        {
            await Clients.Caller.ReceiveNotification($"Notification {notificationId} marked as read.");
        }
        else
        {
            await Clients.Caller.ReceiveNotification($"Error marking notification {notificationId} as read: {result.Error}");
        }
    }
}
