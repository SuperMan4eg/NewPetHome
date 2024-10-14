using System.Data;

namespace NewPetHome.Volunteers.Application;

public interface IVolunteersUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

    Task SaveChanges(CancellationToken cancellationToken = default);
}