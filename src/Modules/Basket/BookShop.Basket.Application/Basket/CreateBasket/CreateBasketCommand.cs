using BuildingBlocks.Application.Mediator;

namespace BookShop.Basket.Application.Basket.CreateBasket;

public sealed record CreateBasketCommand(
) : ICommand<Guid>;
