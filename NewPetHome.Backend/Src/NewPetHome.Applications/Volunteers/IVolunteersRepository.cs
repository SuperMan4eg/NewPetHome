using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.VolunteersManagement.Entitys;
using NewPetHome.Domain.VolunteersManagement.IDs;

namespace NewPetHome.Applications.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
}