using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Commands.HardDeletePetById;

public record HardDeletePetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;