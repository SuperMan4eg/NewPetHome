using NewPetHome.Core.Abstraction;

namespace NewPetHome.Species.Applications.Queries.GetAllSpeciesWithPagination;

public record GetAllSpeciesWithPaginationQuery(
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;