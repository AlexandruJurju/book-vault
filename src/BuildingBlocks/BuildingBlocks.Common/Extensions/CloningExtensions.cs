using System.Text.Json;

namespace BuildingBlocks.Common.Extensions;

public static class CloningExtensions
{
    public static T DeepCloneJson<T>(this T source)
    {
        return source is null
            ? default
            : JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source));
    }
}
