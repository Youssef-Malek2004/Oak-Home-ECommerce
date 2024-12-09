using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Notifications.Application.Services.SignalR;
using Notifications.Domain.Entities;

namespace Notifications.Api.SignalR;

public class SignalRNotificationService(IHubContext<ChatHub, IChatClient> hubContext) : INotificationService
{
    public async Task SendNotificationToUserAsync(Guid userId, Notification notification)
    {
        await hubContext.Clients.Group(userId.ToString()).ReceiveNotification(JsonSerializer.Serialize(notification));
    }

    public async Task BroadcastNotificationAsync(Notification notification)
    {
        await hubContext.Clients.All.ReceiveNotification(JsonSerializer.Serialize(notification));
    }
}