using NewPetHome.Core.Abstraction;

namespace NewPetHome.Species.Applications.Queries.GetBreedsBySpecieId;

public record GetBreedsBySpecieIdQuery(Guid SpecieId, int Page, int PageSize) : IQuery;