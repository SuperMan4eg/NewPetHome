using NewPetHome.Core.Dtos;

namespace NewPetHome.Species.Applications;

public interface IReadDbContext
{
    IQueryable<SpecieDto> Species { get; }

    IQueryable<BreedDto> Breeds { get; }
}