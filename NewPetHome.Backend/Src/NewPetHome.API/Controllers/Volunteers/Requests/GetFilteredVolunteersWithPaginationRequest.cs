using NewPetHome.Applications.VolunteersManagement.Queries.GetVolunteersWithPagination;

namespace NewPetHome.API.Controllers.Volunteers.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    string? FirstName,
    int? ExperienceFrom,
    int? ExperienceTo,
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() =>
        new(FirstName, ExperienceFrom, ExperienceTo, Page, PageSize);
}