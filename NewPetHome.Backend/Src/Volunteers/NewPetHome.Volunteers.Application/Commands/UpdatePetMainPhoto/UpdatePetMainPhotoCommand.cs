using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetMainPhoto;

public record UpdatePetMainPhotoCommand(Guid VolunteerId, Guid PetId, string FilePath) : ICommand;