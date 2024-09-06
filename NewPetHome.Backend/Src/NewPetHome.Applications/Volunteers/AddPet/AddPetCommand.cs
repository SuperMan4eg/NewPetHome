using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
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
    IEnumerable<FileDto> Files,
    IEnumerable<RequisiteDto> Requisites);