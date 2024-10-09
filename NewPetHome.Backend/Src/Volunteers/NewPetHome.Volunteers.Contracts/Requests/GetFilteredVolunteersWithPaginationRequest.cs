namespace NewPetHome.Volunteers.Contracts.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    string? FirstName,
    int? ExperienceFrom,
    int? ExperienceTo,
    int Page,
    int PageSize);