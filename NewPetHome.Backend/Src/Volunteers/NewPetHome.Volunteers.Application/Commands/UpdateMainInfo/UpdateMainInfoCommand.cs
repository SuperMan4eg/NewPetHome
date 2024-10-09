using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Application.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId,
    FullNameDto FullName,
    string Description,
    string Email,
    int Experience,
    string PhoneNumber) : ICommand;