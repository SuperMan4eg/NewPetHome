using CSharpFunctionalExtensions;
using NewPetHome.Core.Dtos;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Contracts;

public interface IVolunteersContract
{
    Task<Result<PetDto,Error>> IsPetsUsedBreed(Guid breedId,CancellationToken cancellationToken);
    Task<Result<PetDto,Error>> IsPetsUsedSpecie(Guid specieId,CancellationToken cancellationToken);
}