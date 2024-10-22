using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Commands.SoftDeletePetById;

public record SoftDeletePetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;