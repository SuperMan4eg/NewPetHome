using System.Data;

namespace NewPetHome.Applications.Database;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

    Task SaveChanges(CancellationToken cancellationToken = default);
}