using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.VolunteersManagement.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Name,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsCastrated,
    DateTime BirthDate,
    bool IsVaccinated,
    string Status,
    IEnumerable<RequisiteDto> Requisites) : ICommand;