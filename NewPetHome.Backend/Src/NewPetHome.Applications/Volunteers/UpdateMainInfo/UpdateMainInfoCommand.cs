using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId, 
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber);