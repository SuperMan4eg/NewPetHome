using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Commands.DeletePetFiles;

public record DeletePetFilesCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<string> FilePaths) : ICommand;