using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Events;
using Shared.Contracts.Kafka;
using Shared.Contracts.Topics;

namespace Products.Infrastructure.Kafka;

public class KafkaNotificationsService(IKafkaProducerService kafkaProducerService) : IKafkaNotificationService
{
    public async Task SendNotificationAsync(Notification notification, CancellationToken cancellationToken)
    {
        var notificationRequest = new NotificationRequest { Notification = notification };
        await kafkaProducerService.SendMessageAsync(
            Topics.NotificationRequests.Name, notificationRequest, cancellationToken, "");
    }
}