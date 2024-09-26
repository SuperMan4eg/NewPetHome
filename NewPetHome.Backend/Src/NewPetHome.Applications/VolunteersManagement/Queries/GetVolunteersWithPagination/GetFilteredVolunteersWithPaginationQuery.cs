using NewPetHome.Applications.Abstraction;

namespace NewPetHome.Applications.VolunteersManagement.Queries.GetVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    string? FirstName,
    int? ExperienceFrom,
    int? ExperienceTo,
    int Page,
    int PageSize) : IQuery;