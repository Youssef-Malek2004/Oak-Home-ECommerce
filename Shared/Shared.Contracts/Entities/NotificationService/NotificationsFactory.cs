namespace Shared.Contracts.Entities.NotificationService;

public static class NotificationsFactory
{
    public static Notification GenerateSuccessWebNotificationUser(string userId, string group, string title, string message)
    {
        var notification = new Notification
        {
            Channel = Channels.WebSocket.Name,
            CreatedAt = DateTime.UtcNow,
            Group = group,
            IsDelivered = false,
            IsRead = false,
            SentAt = DateTime.UtcNow,
            Type = Types.Success.Name,
            Title = title,
            UserId = Guid.Parse(userId),
            Message = message
        };
        
        return notification;
    }
    
    public static Notification GenerateSuccessWebNotificationGroup( string group, string title, string message)
    {
        var notification = new Notification
        {
            Channel = Channels.WebSocket.Name,
            CreatedAt = DateTime.UtcNow,
            Group = group,
            IsDelivered = false,
            IsRead = false,
            SentAt = DateTime.UtcNow,
            Type = Types.Success.Name,
            Title = title,
            Message = message
        };
        
        return notification;
    }
    
    public static Notification GenerateErrorWebNotificationUser(string userId, string group,string title, string message)
    {
        var notification = new Notification
        {
            Channel = Channels.WebSocket.Name,
            CreatedAt = DateTime.UtcNow,
            Group = group,
            IsDelivered = false,
            IsRead = false,
            SentAt = DateTime.UtcNow,
            Type = Types.Error.Name,
            Title = title,
            UserId = Guid.Parse(userId),
            Message = message
        };
        
        return notification;
    }
    
    public static Notification GenerateErrorWebNotificationGroup( string group, string title, string message)
    {
        var notification = new Notification
        {
            Channel = Channels.WebSocket.Name,
            CreatedAt = DateTime.UtcNow,
            Group = group,
            IsDelivered = false,
            IsRead = false,
            SentAt = DateTime.UtcNow,
            Type = Types.Error.Name,
            Title = title,
            Message = message
        };
        
        return notification;
    }
}