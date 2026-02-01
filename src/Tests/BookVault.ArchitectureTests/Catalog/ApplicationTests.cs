using ArchUnitNET.TUnit;
using BookVault.ArchitectureTests.Abstractions;

namespace BookVault.ArchitectureTests.Catalog;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

public sealed class ApplicationTests : ArchUnitBaseTest
{
    private const string ApplicationNamespace =
        $"{nameof(BookVault)}.{nameof(Catalog)}.Application.*";

    [Test]
    public void GivenApplicationLayer_WhenCheckingDependencies_ThenShouldNotDependOnMediator()
    {
        var forbiddenTypes = Types().That()
            .HaveFullName("Mediator.ICommand")
            .Or()
            .HaveFullName("Mediator.ICommand`1");

        Classes()
            .That()
            .ResideInNamespaceMatching(ApplicationNamespace)
            .Should()
            .NotDependOnAny(forbiddenTypes)
            .Because("Application must use BuildingBlocks.Application.Mediator.ICommand instead of Mediator.ICommand.")
            .Check(Architecture);
    }
}