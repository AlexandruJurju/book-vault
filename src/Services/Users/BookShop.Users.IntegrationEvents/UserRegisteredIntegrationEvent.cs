using BuildingBlocks.Application.EventBus;

namespace BookShop.Users.IntegrationEvents;

public sealed record UserRegisteredIntegrationEvent(
    Guid Id,
    DateTime OccurredOnUtc,
    Guid UserId,
    string Email
) : IntegrationEvent(Id, OccurredOnUtc);
