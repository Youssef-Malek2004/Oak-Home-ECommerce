using Abstractions.ResultsPattern;

namespace Shared.Contracts.Kafka;

public interface IAdminClientService
{
    Task<Result> AddPartitionsAsync(string topicName, int totalPartitions);

    Task<Result<int>> GetPartitionCountAsync(string topicName);

    Task<Result> CreateTopicAsync(string topicName, int numPartitions, short replicationFactor);

}