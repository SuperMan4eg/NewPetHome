using NewPetHome.Applications.Dtos;
using NewPetHome.Domain.VolunteersManagement.IDs;

namespace NewPetHome.Applications.Volunteers.UpdateMainInfo;

public record UpdateMainInfoRequest(Guid VolunteerId, UpdateMainInfoDto Dto);