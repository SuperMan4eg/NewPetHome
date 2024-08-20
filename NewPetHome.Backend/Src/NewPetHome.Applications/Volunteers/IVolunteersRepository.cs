using NewPetHome.Domain.Volunteers;

namespace NewPetHome.Applications.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Volunteer> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
}