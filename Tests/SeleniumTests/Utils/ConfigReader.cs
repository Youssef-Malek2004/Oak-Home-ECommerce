using Newtonsoft.Json.Linq;

namespace SeleniumTests.Utils;

public static class ConfigReader
{
    private static readonly JObject Config = JObject.Parse(File.ReadAllText("appsettings.json"));

    public static string? GetValue(string key)
    {
        return Config[key]?.ToString();
    }
}