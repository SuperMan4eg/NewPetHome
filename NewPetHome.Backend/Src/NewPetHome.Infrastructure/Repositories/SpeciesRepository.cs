using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NewPetHome.Applications.SpeciesManagement;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement;
using NewPetHome.Domain.SpeciesManagement.IDs;
using NewPetHome.Infrastructure.DbContexts;

namespace NewPetHome.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SpeciesRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task<Result<Guid, Error>> Add(Specie specie, CancellationToken cancellationToken = default)
    {
        var findResult = await GetByName(specie.Name, cancellationToken);
        if (findResult.IsSuccess)
            return Errors.General.AlreadyExists(nameof(Specie), nameof(specie.Name).ToLower(), specie.Name.Value);

        await _writeDbContext.Species.AddAsync(specie, cancellationToken);

        return specie.Id.Value;
    }

    public async Task<Guid> Delete(Specie specie, CancellationToken cancellationToken = default)
    {
        var result = await GetById(specie.Id, cancellationToken);
        if (result.IsSuccess)
            _writeDbContext.Species.Remove(specie);

        return specie.Id;
    }

    public async Task<Result<Specie, Error>> GetById(SpecieId specieId, CancellationToken cancellationToken = default)
    {
        var result = await _writeDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == specieId, cancellationToken);
        if (result is null)
            return Errors.General.NotFound();

        return result;
    }

    public async Task<Result<Specie, Error>> GetByName(Name name, CancellationToken cancellationToken = default)
    {
        var result = await _writeDbContext.Species
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        if (result is null)
            return Errors.General.NotFound();

        return result;
    }
}