using Abstractions.ResultsPattern;
using Notifications.Domain.Entities;

namespace Notifications.Application.Services.Redis;

public interface IRedisService
{
    Task<Result<List<Notification>>> GetNotificationsAsync(Guid userId);
    Task<Result<List<Notification>>> GetUnreadNotificationsAsync(Guid userId);
    Task<Result<List<Notification>>> GetUndeliveredNotificationsAsync(Guid userId);
    Task<Result<List<Notification>>> GetOneWeekOldNotificationsAsync();
    Task<Result<List<Notification>>> GetAllNotificationsAsync(); 
    
    Task<Result> AddNotificationAsync(Guid userId, Notification notification);
    Task<Result> DeleteNotificationAsync(Guid userId, Guid notificationId);
    Task<Result> MarkNotificationAsReadAsync(Guid userId, Guid notificationId);
    Task<Result> MarkNotificationsAsReadAsync(Guid userId, List<Guid> notificationIds);
    Task<Result> MarkNotificationAsDeliveredAsync(Guid userId, Guid notificationId);
    Task<Result> MarkNotificationsAsDeliveredAsync(Guid userId, List<Guid> notificationIds);
}