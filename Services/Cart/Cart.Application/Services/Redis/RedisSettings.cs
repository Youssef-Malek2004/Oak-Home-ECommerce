namespace Cart.Application.Services.Redis;

public class RedisSettings
{
    public string ConnectionStringLocal { get; set; } = string.Empty;
    public string ConnectionStringDocker { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
}