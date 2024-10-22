using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetInfo;

public record UpdatePetInfoCommand(
    Guid VolunteerId,
    Guid PetId,
    string Name,
    string Description,
    Guid SpecieId,
    Guid BreedId,
    string Color,
    string HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsCastrated,
    DateTime BirthDate,
    bool IsVaccinated,
    IEnumerable<RequisiteDto> Requisites) : ICommand;