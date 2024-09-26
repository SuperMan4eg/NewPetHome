using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.VolunteersManagement.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(
    Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files) : ICommand;