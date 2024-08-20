using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string FirstName,
    string LastName,
    int Experience,
    string Description,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetwork,
    List<RequisiteDto> Requisites);