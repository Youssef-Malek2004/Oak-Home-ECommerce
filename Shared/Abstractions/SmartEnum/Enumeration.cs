using System.Collections.Concurrent;
using System.Reflection;

namespace Abstractions.SmartEnum;

public class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly ConcurrentDictionary<int, TEnum> Enumerations = CreateEnumerations();
    protected Enumeration(int value, string name)
    {
        Id = value;
        Name = name;
    }
    public int Id { get; protected init; }
    public string Name { get; protected init; } = string.Empty;

    public static TEnum? FromValue(int value)
    {
        return Enumerations.GetValueOrDefault(value);
    }
    
    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.SingleOrDefault(e => e.Name == name);
    }
    public static TEnum[] GetValues()
    {
        return Enumerations.Values.ToArray();
    }
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null) return false;
        return GetType() == other.GetType() && Id == other.Id;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static ConcurrentDictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(info =>
                enumerationType.IsAssignableFrom(info.FieldType))
            .Select(info =>
                (TEnum)info.GetValue(default)!);

        return new ConcurrentDictionary<int, TEnum>(fieldsForType.ToDictionary(x => x.Id));
    }
}