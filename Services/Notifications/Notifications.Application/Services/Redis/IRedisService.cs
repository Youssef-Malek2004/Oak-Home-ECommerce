using Abstractions.ResultsPattern;
using Shared.Contracts.Entities;
using Shared.Contracts.Entities.NotificationService;

namespace Notifications.Application.Services.Redis;

public interface IRedisService
{
    Task<Result<List<Notification>>> GetNotificationsAsync(Guid userId, string group);
    Task<Result<List<Notification>>> GetUnreadNotificationsAsync(Guid userId, string group);
    Task<Result<List<Notification>>> GetUndeliveredNotificationsAsync(Guid userId, string group);
    Task<Result<List<Notification>>> GetOneWeekOldNotificationsAsync();
    Task<Result<List<Notification>>> GetAllNotificationsAsync();
    Task<Result> AddUserToGroupAsync(Guid userId, string group);
    Task<Result> AddNotificationAsync(Guid userId, Notification notification);
    Task<Result> AddNotificationToGroupAsync(Notification notification);
    Task<Result> DeleteNotificationAsync(Guid userId, string group, Guid notificationId);
    Task<Result> MarkNotificationAsReadAsync(Guid userId, string group, Guid notificationId);
    Task<Result> MarkNotificationsAsReadAsync(Guid userId, string group, List<Guid> notificationIds);
    Task<Result> MarkNotificationAsDeliveredAsync(Guid userId, string group, Guid notificationId);
    Task<Result> MarkNotificationsAsDeliveredAsync(Guid userId, string group, List<Guid> notificationIds);
}