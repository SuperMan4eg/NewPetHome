using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.VolunteersManagement.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber) : ICommand;