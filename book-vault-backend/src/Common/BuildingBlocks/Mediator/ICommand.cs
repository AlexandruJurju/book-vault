using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Mediator;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}