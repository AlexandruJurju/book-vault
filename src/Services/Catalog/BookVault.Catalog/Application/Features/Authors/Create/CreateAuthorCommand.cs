
using BuildingBlocks.Application.Mediator;

namespace BookVault.Catalog.Application.Features.Authors.Create;

public sealed record CreateAuthorCommand(
    string Name
) : ICommand<Guid>;