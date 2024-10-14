using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NewPetHome.Core.Dtos;
using NewPetHome.SharedKernel;
using NewPetHome.Volunteers.Application;
using NewPetHome.Volunteers.Contracts;

namespace NewPetHome.Volunteers.Presentation;

public class VolunteersContract(IVolunteersReadDbContext readDbContext) : IVolunteersContract
{
    public async Task<Result<PetDto, Error>> IsPetsUsedBreed(Guid breedId, CancellationToken cancellationToken)
    {
        var result = await readDbContext.Pets.FirstOrDefaultAsync(p => p.BreedId == breedId, cancellationToken);
        if (result is null)
            return Errors.General.NotFound();

        return result;
    }

    public async Task<Result<PetDto, Error>> IsPetsUsedSpecie(Guid specieId, CancellationToken cancellationToken)
    {
        var result = await readDbContext.Pets.FirstOrDefaultAsync(p => p.SpeciesId == specieId, cancellationToken);
        if (result is null)
            return Errors.General.NotFound();

        return result;
    }
}