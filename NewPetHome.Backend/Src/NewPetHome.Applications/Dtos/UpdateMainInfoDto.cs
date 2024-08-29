using NewPetHome.Domain.VolunteersManagement.IDs;

namespace NewPetHome.Applications.Dtos;

public record UpdateMainInfoDto(
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber);