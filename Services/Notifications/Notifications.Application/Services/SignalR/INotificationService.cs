using Notifications.Domain.Entities;

namespace Notifications.Application.Services.SignalR;

public interface INotificationService
{
    Task SendNotificationToUserAsync(Guid userId, Notification notification);
    Task BroadcastNotificationAsync(Notification notification);
}