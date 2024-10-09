using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Application.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(
    Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files) : ICommand;