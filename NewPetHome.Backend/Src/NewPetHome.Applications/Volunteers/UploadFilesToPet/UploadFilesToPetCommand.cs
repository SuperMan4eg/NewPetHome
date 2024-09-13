using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files);