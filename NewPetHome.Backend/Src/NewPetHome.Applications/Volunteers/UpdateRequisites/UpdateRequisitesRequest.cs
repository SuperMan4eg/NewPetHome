using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.UpdateRequisites;

public record UpdateRequisitesRequest(Guid VolunteerId, UpdateRequisitesDto Dto);