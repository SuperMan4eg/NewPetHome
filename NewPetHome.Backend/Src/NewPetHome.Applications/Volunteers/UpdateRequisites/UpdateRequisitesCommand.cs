using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites);