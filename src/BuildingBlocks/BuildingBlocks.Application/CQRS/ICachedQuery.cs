namespace BuildingBlocks.Application.CQRS;

public interface ICachedQuery
{
    string Key { get; }
    TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery
{
}
