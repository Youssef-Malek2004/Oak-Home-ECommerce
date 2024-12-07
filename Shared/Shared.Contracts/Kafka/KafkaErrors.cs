using Abstractions.ResultsPattern;
using Abstractions.SmartEnum;

namespace Shared.Contracts.Kafka;

public class KafkaErrors : Enumeration<KafkaErrors>
{
    protected KafkaErrors(int value, string name) : base(value, name)
    {
    }
    
    public static Error CantFetchPartitionCount(string topic) =>
        new Error("KafkaErrors.CantFetchPartitionCount", $"Cant Fetch Partition Count for Topic: {topic}");
    public static Error FailedToAddPartitions(string topic) =>
        new Error("KafkaErrors.FailedToAddPartitions", $"Failed to Add Partitions for Topic: {topic}");
    public static Error TopicAlreadyExists(string topic) =>
        new Error("KafkaErrors.TopicAlreadyExists", $"Topic: {topic} Already Exists");
    public static Error FailedToCreateTopic(string topic) =>
        new Error("KafkaErrors.FailedToCreateTopic", $"Topic: {topic} Failed to Create");
}