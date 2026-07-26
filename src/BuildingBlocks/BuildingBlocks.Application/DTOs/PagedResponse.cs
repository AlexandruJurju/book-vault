namespace BuildingBlocks.Application.DTOs;

public sealed record PaginatedResult<TItem>(
    int PageNumber,
    int PageSize,
    int TotalCount,
    IReadOnlyList<TItem> Items
)
{
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
