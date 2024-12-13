using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared.Contracts.Kafka;

namespace Products.Infrastructure.Kafka;

public class KafkaConsumerService(IOptions<KafkaSettings> settings, IAdminClient adminClient) : IKafkaConsumerService
{
    private const string InitialGroupInstanceId = "products-service-instance-";
    public void StartConsuming<T>(string topic ,string groupInstanceName,
        Func<ConsumeResult<string,string>, Task> processMessage, CancellationToken stoppingToken)
    {
        var kafkaConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");
        
        ConsumerConfig config = new()
        {
            BootstrapServers = kafkaConnectionString ?? "localhost:9092",
            GroupId = settings.Value.GroupId,
            AllowAutoCreateTopics = true,
            EnableAutoCommit = true,
            GroupInstanceId = InitialGroupInstanceId + groupInstanceName, //Works Fine without it now :)
            AutoOffsetReset = AutoOffsetReset.Latest,
        };
        
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        
        if (!metadata.Topics.Any(t => t.Topic == topic && t.Error.Code == ErrorCode.NoError))
        {
            Console.WriteLine($"Error: Topic '{topic}' does not exist.");
            return;
        }
        
        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        
        consumer.Subscribe(topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);

                if (consumeResult is null)
                {
                    continue;
                }
                
                processMessage(consumeResult).Wait(stoppingToken);
                
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Kafka consumer: {consumer.MemberId} operation canceled.");
        }
        finally
        {
            try
            {
                consumer.Close(); 
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Kafka consumer already disposed.");
            }
        }
    }
}