using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.Volunteers.UpdateMainInfo;

namespace NewPetHome.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber)
{
    public UpdateMainInfoCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, FullName, Description, Email, Experience, PhoneNumber);
}