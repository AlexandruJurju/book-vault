using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;

namespace BookVault.ArchitectureTests.Abstractions;

public abstract class ArchUnitBaseTest : BaseTest
{
    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            CatalogAssembly,
            typeof(Mediator.ICommand).Assembly
        )
        .Build();
    
    protected static readonly IObjectProvider<IType> CatalogServiceTypes = ArchRuleDefinition
        .Types()
        .That()
        .ResideInAssembly(CatalogAssembly)
        .And()
        .DoNotResideInNamespaceMatching("Microsoft.CodeCoverage.*")
        .As(nameof(Catalog));
}