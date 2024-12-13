namespace Shared.Contracts.Entities.NotificationService;
public interface IKafkaNotificationService
{
    Task SendNotificationAsync(Notification notification, CancellationToken cancellationToken);
}