namespace Notifications.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid(); 

    public string Title { get; set; } = string.Empty; // Title or subject of the notification
    public string Message { get; set; } = string.Empty; // Main content of the notification

    public string Type { get; set; } = string.Empty; // e.g., "info", "warning", "error", "success"

    public Guid? UserId { get; set; } // Targeted user (optional, null for broadcast)
    public string Group { get; set; } = string.Empty; // Group for grouped notifications (e.g., "Admin")

    public string Channel { get; set; } = string.Empty; // Delivery channel (e.g., "Email", "Push", "SMS", "WebSocket")

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // When the notification was created
    public DateTime? SentAt { get; set; } // When the notification was sent (optional)

    public bool IsRead { get; set; } = false; // Whether the notification has been read
    public bool IsDelivered { get; set; } = false; // Whether the notification was successfully delivered
}
