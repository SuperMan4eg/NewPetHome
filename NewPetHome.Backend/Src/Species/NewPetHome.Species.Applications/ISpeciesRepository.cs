using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Domain;

namespace NewPetHome.Species.Applications;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> Add(Specie specie, CancellationToken cancellationToken = default);

    Task<Guid> Delete(Specie specie, CancellationToken cancellationToken = default);

    void Save(Specie specie, CancellationToken cancellationToken = default);
    
    Task<Result<Specie, Error>> GetSpecieById(SpecieId specieId, CancellationToken cancellationToken = default);

    Task<Result<Specie, Error>> GetSpecieByName(Name name, CancellationToken cancellationToken = default);

    
}