using NewPetHome.Applications.Dtos;

namespace NewPetHome.API.Contracts;

public record AddPetRequest(
    string Name,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    int Weight,
    int Height,
    string PhoneNumber,
    bool IsCastrated,
    DateOnly BirthDate,
    bool IsVaccinated,
    string Status,
    IFormFileCollection Files,
    IEnumerable<RequisiteDto> Requisites);