using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NewPetHome.Core.Dtos;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Applications;
using NewPetHome.Species.Contracts;

namespace NewPetHome.Species.Presentation;

public class SpeciesContract(ISpeciesReadDbContext readDbContext) : ISpeciesContract
{
    public async Task<Result<SpecieDto, Error>> GetSpecieById(SpecieId id, CancellationToken cancellationToken = default)
    {
        var specie = await readDbContext.Species
            .Include(s=>s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (specie is null)
            return Errors.General.NotFound(id);

        return specie;
    }
}