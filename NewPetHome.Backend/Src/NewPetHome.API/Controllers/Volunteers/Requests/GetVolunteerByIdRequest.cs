using NewPetHome.Applications.VolunteersManagement.Queries.GetVolunteerById;

namespace NewPetHome.API.Controllers.Volunteers.Requests;

public record GetVolunteerByIdRequest(Guid Id)
{
    public GetVolunteerByIdQuery ToQuery() =>
        new(Id);
}