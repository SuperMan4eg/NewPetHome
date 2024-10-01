using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Applications.SpeciesManagement;

public interface ISpeciesRepository
{
    Task<Result<Guid, Error>> Add(Specie specie, CancellationToken cancellationToken = default);

    Task<Guid> Delete(Specie specie, CancellationToken cancellationToken = default);

    Task<Result<Specie, Error>> GetById(SpecieId specieId, CancellationToken cancellationToken = default);

    Task<Result<Specie, Error>> GetByName(Name name, CancellationToken cancellationToken = default);
}