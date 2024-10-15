using CSharpFunctionalExtensions;
using NewPetHome.Core.Dtos;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects.Ids;

namespace NewPetHome.Species.Contracts;

public interface ISpeciesContract
{
    Task<Result<SpecieDto, Error>> GetSpecieById(SpecieId id, CancellationToken cancellationToken = default);
}