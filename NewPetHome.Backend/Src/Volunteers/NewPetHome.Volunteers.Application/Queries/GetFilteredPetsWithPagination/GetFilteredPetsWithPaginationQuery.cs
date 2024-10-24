using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Queries.GetFilteredPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    Guid? VolunteerId,
    string? Name,
    int? AgeFrom,
    int? AgeTo,
    int? WeightFrom,
    int? WeightTo,
    int? HeightFrom,
    int? HeightTo,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Color,
    bool? IsCastrated,
    bool? IsVaccinated,
    string? Status,
    string? City,
    string? Street,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize):IQuery;