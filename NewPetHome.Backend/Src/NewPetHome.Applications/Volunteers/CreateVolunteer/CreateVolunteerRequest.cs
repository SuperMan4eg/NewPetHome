using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetworks,
    List<RequisiteDto> Requisites);