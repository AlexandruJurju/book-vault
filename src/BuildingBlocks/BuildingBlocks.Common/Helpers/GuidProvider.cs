namespace BuildingBlocks.Common.Helpers;

public static class GuidProvider
{
    public static Guid NewGuid() => Guid.CreateVersion7();
}
