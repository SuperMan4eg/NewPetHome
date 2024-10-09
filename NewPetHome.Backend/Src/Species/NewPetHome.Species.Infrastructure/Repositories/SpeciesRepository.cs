using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Applications;
using NewPetHome.Species.Domain;
using NewPetHome.Species.Infrastructure.DbContexts;

namespace NewPetHome.Species.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly SpeciesWriteDbContext _speciesWriteDbContext;

    public SpeciesRepository(SpeciesWriteDbContext speciesWriteDbContext)
    {
        _speciesWriteDbContext = speciesWriteDbContext;
    }

    public async Task<Result<Guid, Error>> Add(Specie specie, CancellationToken cancellationToken = default)
    {
        var findResult = await GetByName(specie.Name, cancellationToken);
        if (findResult.IsSuccess)
            return Errors.General.AlreadyExists(nameof(Specie), nameof(Name).ToLower(), specie.Name.Value);

        await _speciesWriteDbContext.Species.AddAsync(specie, cancellationToken);

        return specie.Id.Value;
    }

    public async Task<Guid> Delete(Specie specie, CancellationToken cancellationToken = default)
    {
        var result = await GetById(specie.Id, cancellationToken);
        if (result.IsSuccess)
            _speciesWriteDbContext.Species.Remove(specie);

        return specie.Id;
    }

    public async Task<Result<Specie, Error>> GetById(SpecieId specieId, CancellationToken cancellationToken = default)
    {
        var result = await _speciesWriteDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == specieId, cancellationToken);
        if (result is null)
            return Errors.General.NotFound();

        return result;
    }

    public async Task<Result<Specie, Error>> GetByName(Name name, CancellationToken cancellationToken = default)
    {
        var result = await _speciesWriteDbContext.Species
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        if (result is null)
            return Errors.General.NotFound();

        return result;
    }
}