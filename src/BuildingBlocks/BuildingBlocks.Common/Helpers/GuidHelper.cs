namespace BuildingBlocks.Common.Helpers;

public static class GuidHelper
{
    public static Guid NewGuid()
    {
        return Guid.CreateVersion7();
    }
}
