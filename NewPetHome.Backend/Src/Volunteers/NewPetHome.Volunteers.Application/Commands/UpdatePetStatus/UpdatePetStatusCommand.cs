using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetStatus;

public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string Status) : ICommand;