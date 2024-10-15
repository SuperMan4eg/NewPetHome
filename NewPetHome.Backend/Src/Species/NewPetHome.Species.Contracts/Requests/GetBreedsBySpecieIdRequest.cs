namespace NewPetHome.Species.Contracts.Requests;

public record GetBreedsBySpecieIdRequest(Guid SpecieId, int Page, int PageSize);