using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Products.Application.KafkaSettings;

namespace Products.Infrastructure.Kafka;

public class KafkaConsumerService(IOptions<KafkaSettings> settings)
{
    
    public static void PrintAllEnvironmentVariables()
    {
        Console.WriteLine("Environment Variables:");
        foreach (System.Collections.DictionaryEntry variable in Environment.GetEnvironmentVariables())
        {
            Console.WriteLine($"{variable.Key}: {variable.Value}");
        }
    }
    
    private const string InitialGroupInstanceId = "products-service-instance";
    public void StartConsuming<T>(string topic,string groupInstanceName, Func<ConsumeResult<string,string>, Task> processMessage, CancellationToken stoppingToken)
    {
        var kafkaConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");

        PrintAllEnvironmentVariables();
        
        ConsumerConfig config = new()
        {
            BootstrapServers = kafkaConnectionString ?? "localhost:9092",
            GroupId = settings.Value.GroupId,
            AllowAutoCreateTopics = true,
            GroupInstanceId = InitialGroupInstanceId + groupInstanceName,
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        
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
            Console.WriteLine($"Kafka consumer: {consumer.Name} operation canceled.");
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