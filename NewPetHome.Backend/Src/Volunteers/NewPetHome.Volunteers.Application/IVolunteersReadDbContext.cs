using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Application;

public interface IVolunteersReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }

    IQueryable<PetDto> Pets { get; }
}