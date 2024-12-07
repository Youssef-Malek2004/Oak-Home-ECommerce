using Abstractions.ResultsPattern;
using Abstractions.SmartEnum;

namespace Notifications.Domain.Errors;

public static class NotificationErrors
{
    public static Error FailedToGetNotifications(Guid userId) =>
        new Error("NotificationErrors.FailedToGetNotifications", $"Failed to get notifications for user with ID: {userId}");

    public static Error FailedToGetUnreadNotifications(Guid userId) =>
        new Error("NotificationErrors.FailedToGetUnreadNotifications", $"Failed to get unread notifications for user with ID: {userId}");

    public static Error FailedToGetOneWeekOldNotifications() =>
        new Error("NotificationErrors.FailedToGetOneWeekOldNotifications", "Failed to get one-week-old notifications.");

    public static Error FailedToGetAllNotifications() =>
        new Error("NotificationErrors.FailedToGetAllNotifications", "Failed to get all notifications.");

    public static Error FailedToAddNotification(Guid userId) =>
        new Error("NotificationErrors.FailedToAddNotification", $"Failed to add notification for user with ID: {userId}");

    public static Error NotificationNotFound(Guid notificationId) =>
        new Error("NotificationErrors.NotificationNotFound", $"Notification with ID: {notificationId} was not found.");

    public static Error FailedToDeleteNotification(Guid userId, Guid notificationId) =>
        new Error("NotificationErrors.FailedToDeleteNotification", $"Failed to delete notification with ID: {notificationId} for user with ID: {userId}");

    public static Error FailedToMarkNotificationAsRead(Guid userId, Guid notificationId) =>
        new Error("NotificationErrors.FailedToMarkNotificationAsRead", $"Failed to mark notification with ID: {notificationId} as read for user with ID: {userId}");

    public static Error FailedToMarkNotificationsAsRead(Guid userId) =>
        new Error("NotificationErrors.FailedToMarkNotificationsAsRead", $"Failed to mark notifications as read for user with ID: {userId}");

    public static Error FailedToDeleteNotificationWithoutUser(Guid notificationId) =>
        new Error("NotificationErrors.FailedToDeleteNotificationWithoutUser", $"Failed to delete notification with ID: {notificationId}");
    
}
