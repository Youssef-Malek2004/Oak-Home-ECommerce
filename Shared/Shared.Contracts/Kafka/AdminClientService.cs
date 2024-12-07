using Abstractions.ResultsPattern;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace Shared.Contracts.Kafka;

public class AdminClientService(IAdminClient adminClient) : IDisposable, IAdminClientService
{

    public async Task<Result> AddPartitionsAsync(string topicName, int totalPartitions)
    {
        try
        {
            var partitionsSpecification = new List<PartitionsSpecification>
            {
                new PartitionsSpecification
                {
                    Topic = topicName,
                    IncreaseTo = totalPartitions
                }
            };

            await adminClient.CreatePartitionsAsync(partitionsSpecification);
            Console.WriteLine(
                $"Successfully added partitions to topic '{topicName}'. Total partitions: {totalPartitions}");
        }
        catch (CreatePartitionsException ex)
        {
            Console.WriteLine($"Failed to add partitions to topic '{topicName}': {ex.Results[0].Error.Reason}");
            return Result.Failure(KafkaErrors.FailedToAddPartitions(topicName));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        return Result.Success();
    }

    public Task<Result<int>> GetPartitionCountAsync(string topicName)
    {
        try
        {
            var metadata = adminClient.GetMetadata(topicName, TimeSpan.FromSeconds(10));
            return Task.FromResult(Result<int>.Success(metadata.Topics[0].Partitions.Count));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching partition count: {ex.Message}");
            return Task.FromResult(Result<int>.Failure(KafkaErrors.CantFetchPartitionCount(topicName)));
        }
    }

    public async Task<Result> CreateTopicAsync(string topicName, int numPartitions, short replicationFactor)
    {
        try
        {
            var topicSpecification = new TopicSpecification
            {
                Name = topicName,
                NumPartitions = numPartitions,
                ReplicationFactor = replicationFactor
            };

            await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpecification });
            Console.WriteLine(
                $"Successfully created topic '{topicName}' with {numPartitions} partitions and {replicationFactor} replication factor.");
        }
        catch (CreateTopicsException ex)
        {
            foreach (var result in ex.Results)
            {
                if (result.Error.Code == ErrorCode.TopicAlreadyExists)
                {
                    Console.WriteLine($"Topic '{topicName}' already exists.");
                    
                    return Result.Failure(KafkaErrors.TopicAlreadyExists(topicName));
                }
                else
                {
                    Console.WriteLine($"Failed to create topic '{topicName}': {result.Error.Reason}");
                    
                    return Result.Failure(KafkaErrors.FailedToCreateTopic(topicName));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while creating topic '{topicName}': {ex.Message}");
        }

        return Result.Success();
    }


    public void Dispose()
    {
        // GC.SuppressFinalize(this);
        // adminClient.Dispose(); //Already Being Disposed by another Service
    }
}