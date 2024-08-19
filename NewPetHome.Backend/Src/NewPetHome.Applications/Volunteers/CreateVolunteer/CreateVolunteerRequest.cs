using NewPetHome.Domain.Volunteers;

namespace NewPetHome.Applications.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string FullName,
    int Experience,
    string Description,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetwork,
    List<RequisiteDto> Requisites);

public record RequisiteDto(string Name, string Description);

public record SocialNetworkDto(string Name, string Url);