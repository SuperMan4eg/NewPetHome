namespace NewPetHome.Species.Contracts.Requests;

public record GetAllSpeciesWithPaginationRequest(string? SortDirection, int Page, int PageSize);