namespace Products.Application.Services.Kafka;

public interface IKafkaProducerService
{
    Task SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken, string eventType);
}