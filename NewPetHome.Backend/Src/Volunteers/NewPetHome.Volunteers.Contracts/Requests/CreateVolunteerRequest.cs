using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Contracts.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetworks,
    List<RequisiteDto> Requisites);