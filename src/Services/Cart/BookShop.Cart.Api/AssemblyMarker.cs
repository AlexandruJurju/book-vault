using System.Reflection;

namespace BookShop.Cart.Api;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
