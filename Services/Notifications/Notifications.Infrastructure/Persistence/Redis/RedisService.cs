using System.Text.Json;
using Abstractions.ResultsPattern;
using Notifications.Application.Services.Redis;
using Notifications.Domain.Entities;
using Notifications.Domain.Errors;
using StackExchange.Redis;

namespace Notifications.Infrastructure.Persistence.Redis;

public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private readonly IDatabase _db = redis.GetDatabase();
    private static string GetUserKey(Guid userId) => $"notifications:{userId}";

    public async Task<Result<List<Notification>>> GetNotificationsAsync(Guid userId)
    {
        try
        {
            var key = GetUserKey(userId);
            var notifications = await _db.ListRangeAsync(key);

            var result = notifications.Select(n => JsonSerializer.Deserialize<Notification>(n.ToString()))
                                       .Where(n => n != null)
                                       .ToList();
            return Result<List<Notification>>.Success(result!);
        }
        catch (Exception)
        {
            return Result<List<Notification>>.Failure(NotificationErrors.FailedToGetNotifications(userId));
        }
    }

    public async Task<Result<List<Notification>>> GetUnreadNotificationsAsync(Guid userId)
    {
        try
        {
            var notificationsResult = await GetNotificationsAsync(userId);
            if (!notificationsResult.IsSuccess) return notificationsResult;

            var unreadNotifications = notificationsResult.Value!.Where(n => !n.IsRead).ToList();
            return Result<List<Notification>>.Success(unreadNotifications);
        }
        catch (Exception)
        {
            return Result<List<Notification>>.Failure(NotificationErrors.FailedToGetUnreadNotifications(userId));
        }
    }

    public async Task<Result<List<Notification>>> GetUndeliveredNotificationsAsync(Guid userId)
    {
        try
        {
            var notificationsResult = await GetNotificationsAsync(userId);
            if (!notificationsResult.IsSuccess) return notificationsResult;

            var unreadNotifications = notificationsResult.Value!.Where(n => !n.IsDelivered).ToList();
            return Result<List<Notification>>.Success(unreadNotifications);
        }
        catch (Exception)
        {
            return Result<List<Notification>>.Failure(NotificationErrors.FailedToGetUndeliveredNotifications(userId));
        }
    }

    public async Task<Result<List<Notification>>> GetOneWeekOldNotificationsAsync()
    {
        try
        {
            var server = redis.GetServer(redis.GetEndPoints().First());
            var keys = server.Keys(pattern: "notifications:*");

            var oldNotifications = new List<Notification>();

            foreach (var key in keys)
            {
                var notifications = await _db.ListRangeAsync(key);
                oldNotifications.AddRange(
                    notifications.Select(n => JsonSerializer.Deserialize<Notification>(n.ToString()))
                                 .Where(n => n != null && n.CreatedAt <= DateTime.UtcNow.AddDays(-7))!);
            }

            return Result<List<Notification>>.Success(oldNotifications);
        }
        catch (Exception )
        {
            return Result<List<Notification>>.Failure(NotificationErrors.FailedToGetOneWeekOldNotifications());
        }
    }

    public async Task<Result<List<Notification>>> GetAllNotificationsAsync()
    {
        try
        {
            var server = redis.GetServer(redis.GetEndPoints().First());
            var keys = server.Keys(pattern: "notifications:*");

            var allNotifications = new List<Notification>();

            foreach (var key in keys)
            {
                var notifications = await _db.ListRangeAsync(key);
                allNotifications.AddRange(
                    notifications.Select(n => JsonSerializer.Deserialize<Notification>(n.ToString()))
                                 .Where(n => n != null)!);
            }

            return Result<List<Notification>>.Success(allNotifications);
        }
        catch (Exception)
        {
            return Result<List<Notification>>.Failure(NotificationErrors.FailedToGetAllNotifications());
        }
    }

    public async Task<Result> AddNotificationAsync(Guid userId, Notification notification)
    {
        try
        {
            var key = GetUserKey(userId);
            var serializedNotification = JsonSerializer.Serialize(notification);
            await _db.ListRightPushAsync(key, serializedNotification);
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToAddNotification(userId));
        }
    }

    public async Task<Result> DeleteNotificationAsync(Guid userId, Guid notificationId)
    {
        try
        {
            var key = GetUserKey(userId);
            var notifications = await _db.ListRangeAsync(key);

            foreach (var n in notifications)
            {
                var notification = JsonSerializer.Deserialize<Notification>(n.ToString());
                if (notification?.Id == notificationId)
                {
                    await _db.ListRemoveAsync(key, n);
                    return Result.Success();
                }
            }

            return Result.Failure(NotificationErrors.NotificationNotFound(notificationId));
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToDeleteNotification(userId,notificationId));
        }
    }

    public async Task<Result> MarkNotificationAsReadAsync(Guid userId, Guid notificationId)
    {
        try
        {
            var key = GetUserKey(userId);
            var notifications = await _db.ListRangeAsync(key);

            for (int i = 0; i < notifications.Length; i++)
            {
                var notification = JsonSerializer.Deserialize<Notification>(notifications[i].ToString());
                if (notification?.Id == notificationId)
                {
                    notification.IsRead = true;
                    var updatedNotification = JsonSerializer.Serialize(notification);
                    await _db.ListSetByIndexAsync(key, i, updatedNotification);
                    return Result.Success();
                }
            }

            return Result.Failure(NotificationErrors.NotificationNotFound(notificationId));
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToMarkNotificationAsRead(userId,notificationId));
        }
    }

    public async Task<Result> MarkNotificationsAsReadAsync(Guid userId, List<Guid> notificationIds)
    {
        try
        {
            foreach (var notificationId in notificationIds)
            {
                var result = await MarkNotificationAsReadAsync(userId, notificationId);
                if (!result.IsSuccess) return result;
            }

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToMarkNotificationsAsRead(userId));
        }
    }

    public async Task<Result> MarkNotificationAsDeliveredAsync(Guid userId, Guid notificationId)
    {
        try
        {
            var key = GetUserKey(userId);
            var notifications = await _db.ListRangeAsync(key);

            for (int i = 0; i < notifications.Length; i++)
            {
                var notification = JsonSerializer.Deserialize<Notification>(notifications[i].ToString());
                if (notification?.Id == notificationId)
                {
                    notification.IsDelivered = true;
                    var updatedNotification = JsonSerializer.Serialize(notification);
                    await _db.ListSetByIndexAsync(key, i, updatedNotification);
                    return Result.Success();
                }
            }

            return Result.Failure(NotificationErrors.NotificationNotFound(notificationId));
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToMarkNotificationAsRead(userId,notificationId));
        }
    }

    public async Task<Result> MarkNotificationsAsDeliveredAsync(Guid userId, List<Guid> notificationIds)
    {
        try
        {
            foreach (var notificationId in notificationIds)
            {
                var result = await MarkNotificationAsReadAsync(userId, notificationId);
                if (!result.IsSuccess) return result;
            }

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(NotificationErrors.FailedToMarkNotificationsAsRead(userId));
        }
    }
}
