using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Queries.GetVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? FirstName,
    int? ExperienceFrom,
    int? ExperienceTo,
    int Page,
    int PageSize) : IQuery;