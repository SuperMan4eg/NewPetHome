using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.Volunteers.Create;

namespace NewPetHome.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetworks,
    List<RequisiteDto> Requisites)
{
    public CreateVolunteerCommand ToCommand() =>
        new(FullName, Description, Email, Experience, PhoneNumber, SocialNetworks, Requisites);
}