using System.Reflection;

namespace BookShop.Catalog.Api;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
