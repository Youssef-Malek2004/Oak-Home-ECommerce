using Abstractions.SmartEnum;

namespace Shared.Contracts.Topics;

public class Topics : Enumeration<Topics>
{
    private Topics(int value, string name) : base(value, name)
    {
    }

    public static readonly Topics ProductCreatedTopic = new Topics(1, "ProductCreatedTopic");
}