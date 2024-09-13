using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.Volunteers.UpdateRequisites;

namespace NewPetHome.API.Controllers.Volunteers.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites)
{
    public UpdateRequisitesCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, Requisites);
}