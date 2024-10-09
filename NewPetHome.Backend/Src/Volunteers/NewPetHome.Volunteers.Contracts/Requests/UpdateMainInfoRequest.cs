using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Contracts.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber);