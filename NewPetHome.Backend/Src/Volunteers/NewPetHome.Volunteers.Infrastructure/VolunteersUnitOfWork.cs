using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using NewPetHome.Core.Abstraction;
using NewPetHome.Volunteers.Application;
using NewPetHome.Volunteers.Infrastructure.DbContexts;

namespace NewPetHome.Volunteers.Infrastructure;

public class VolunteersUnitOfWork : IVolunteersUnitOfWork
{
    private readonly VolunteersWriteDbContext _dbContext;

    public VolunteersUnitOfWork(VolunteersWriteDbContext dbContext)
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