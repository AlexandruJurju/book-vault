using System.Reflection;

namespace BookVault.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly CatalogAssembly = typeof(Program).Assembly;
}