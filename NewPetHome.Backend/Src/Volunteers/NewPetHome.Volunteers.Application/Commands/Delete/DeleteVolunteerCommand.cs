using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;