using Ardalis.Result;
using BuildingBlocks.Application.Mediator;

namespace BookShop.Basket.Application.Basket.CreateBasket;

// todo: make this internal
public sealed class CreateBasketCommandHandler
    : ICommandHandler<CreateBasketCommand, Guid>
{
    public async ValueTask<Result<Guid>> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
