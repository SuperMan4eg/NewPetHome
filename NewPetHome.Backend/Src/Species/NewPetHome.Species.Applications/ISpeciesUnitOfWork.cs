using System.Data;

namespace NewPetHome.Species.Applications;

public interface ISpeciesUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);

    Task SaveChanges(CancellationToken cancellationToken = default);
}