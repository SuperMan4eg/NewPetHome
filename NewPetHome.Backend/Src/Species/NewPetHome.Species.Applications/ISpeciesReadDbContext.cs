using NewPetHome.Core.Dtos;

namespace NewPetHome.Species.Applications;

public interface ISpeciesReadDbContext
{
    IQueryable<SpecieDto> Species { get; }

    IQueryable<BreedDto> Breeds { get; }
}