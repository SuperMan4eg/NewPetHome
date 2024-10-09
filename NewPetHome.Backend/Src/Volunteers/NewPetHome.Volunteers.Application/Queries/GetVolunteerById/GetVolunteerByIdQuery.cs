using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId): IQuery;