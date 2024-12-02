using Abstractions.SmartEnum;

namespace Shared.Contracts.Topics;

public class Topics : Enumeration<Topics>
{
    protected Topics(int value, string name) : base(value, name)
    {
    }

    public static Topics ProductCreatedTopic = new Topics(1, "ProductCreatedTopic");
}