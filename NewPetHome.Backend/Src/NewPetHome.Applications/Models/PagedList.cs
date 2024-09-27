namespace NewPetHome.Applications.Models;

public class PagedList<T>
{
    public int TotalCount { get; init; }

    public int PageSize { get; init; }

    public int Page { get; init; }

    public IReadOnlyList<T> Items { get; init; }

    public bool HasNextPage => TotalCount > Page * PageSize;

    public bool HasPreviousPage => Page > 1;
}