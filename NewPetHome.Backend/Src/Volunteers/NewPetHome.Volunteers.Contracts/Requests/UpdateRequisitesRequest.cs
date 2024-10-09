using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Contracts.Requests;

public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisites);