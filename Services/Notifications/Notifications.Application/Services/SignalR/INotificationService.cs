using Abstractions.ResultsPattern;
using Notifications.Domain.Entities;

namespace Notifications.Application.Services.SignalR;

public interface INotificationService
{
    Task<Result> SendNotificationToUserAsync(Guid userId, Notification notification);
    Task<Result> BroadcastNotificationAsync(Notification notification);
    Task<Result> SendUndeliveredNotificationsAsync(Guid userId);
    Task<Result> SendUnreadNotificationsAsync(Guid userId);
    
}