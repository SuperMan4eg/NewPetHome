using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetworks,
    List<RequisiteDto> Requisites);