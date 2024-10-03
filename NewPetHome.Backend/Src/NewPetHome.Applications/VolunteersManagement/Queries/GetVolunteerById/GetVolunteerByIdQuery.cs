using NewPetHome.Applications.Abstraction;

namespace NewPetHome.Applications.VolunteersManagement.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId): IQuery;