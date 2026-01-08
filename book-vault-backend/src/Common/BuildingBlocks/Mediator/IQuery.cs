using Ardalis.Result;
using Mediator;

namespace BuildingBlocks.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}