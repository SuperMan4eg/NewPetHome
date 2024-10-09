using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Application.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites) : ICommand;