using ArchUnitNET.TUnit;
using BookVault.ArchitectureTests.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BookVault.ArchitectureTests.Catalog;

public class DomainTests : ArchUnitBaseTest
{
    private const string DomainNamespace =
        $"{nameof(BookVault)}.{nameof(Catalog)}.Domain.*";

    [Test]
    public void GivenCatalogDomain_WhenCheckingClasses_ThenShouldBeSealed()
    {
        Classes()
            .That()
            .ResideInNamespaceMatching(DomainNamespace)
            .Should()
            .BeSealed()
            .Check(Architecture);
    }

    [Test]
    public void GivenOrderingDomain_WhenCheckingClasses_ThenShouldBePublic()
    {
        Classes()
            .That()
            .ResideInNamespaceMatching(DomainNamespace)
            .Should()
            .BePublic()
            .Check(Architecture);
    }

    [Test]
    public void GivenCatalogDomain_WhenCheckingDependencies_ThenShouldNotDependOnApplication()
    {
        Classes()
            .That()
            .ResideInNamespaceMatching(DomainNamespace)
            .Should()
            .NotDependOnAny(
                Types().That().ResideInNamespaceMatching("BookVault.Catalog.Application.*"))
            .Check(Architecture);
    }

    [Test]
    public void GivenCatalogDomain_WhenCheckingDependencies_ThenShouldNotDependOnInfrastructure()
    {
        Classes()
            .That()
            .ResideInNamespaceMatching(DomainNamespace)
            .Should()
            .NotDependOnAny(
                Types().That().ResideInNamespaceMatching("BookVault.Catalog.Infrastructure.*"))
            .Check(Architecture);
    }

    [Test]
    public void GivenCatalogDomain_WhenCheckingDependencies_ThenShouldNotDependOnApi()
    {
        Classes()
            .That()
            .ResideInNamespaceMatching(DomainNamespace)
            .Should()
            .NotDependOnAny(
                Types().That().ResideInNamespaceMatching("BookVault.Catalog.Api.*"))
            .Check(Architecture);
    }
}