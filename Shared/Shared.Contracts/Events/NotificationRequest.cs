using Shared.Contracts.Entities.NotificationService;

namespace Shared.Contracts.Events;

public class NotificationRequest
{
    public required Notification Notification { get; set; }
}