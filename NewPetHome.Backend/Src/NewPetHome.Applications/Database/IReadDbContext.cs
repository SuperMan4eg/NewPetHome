using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }

    IQueryable<PetDto> Pets { get; }
}