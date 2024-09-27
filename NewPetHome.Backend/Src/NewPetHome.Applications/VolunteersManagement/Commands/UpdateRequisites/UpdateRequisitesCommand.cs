using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.VolunteersManagement.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites) : ICommand;