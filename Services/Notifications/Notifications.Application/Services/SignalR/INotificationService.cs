using Abstractions.ResultsPattern;
using Shared.Contracts.Entities;
using Shared.Contracts.Entities.NotificationService;

namespace Notifications.Application.Services.SignalR;

public interface INotificationService
{
    Task<Result> SendNotificationToUserAsync(Guid userId, Notification notification);
    Task<Result> BroadcastNotificationAsync(Notification notification);
    Task<Result> SendNotificationToGroup(Notification notification);
    Task<Result> SendUndeliveredNotificationsAsync(Guid userId, string group);
    Task<Result> SendUnreadNotificationsAsync(Guid userId, string group);
    
}