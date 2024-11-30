namespace Products.Application.Settings;

public class MongoSettings
{
    public string? ConnectionStringLocal { get; set; }
    public string? ConnectionStringDocker { get; set; }
    public string? DatabaseName { get; set; }
}
