using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using NewPetHome.Species.Applications;
using NewPetHome.Species.Infrastructure.DbContexts;

namespace NewPetHome.Species.Infrastructure;

public class SpeciesUnitOfWork : ISpeciesUnitOfWork
{
    private readonly SpeciesWriteDbContext _dbContext;

    public SpeciesUnitOfWork(SpeciesWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}