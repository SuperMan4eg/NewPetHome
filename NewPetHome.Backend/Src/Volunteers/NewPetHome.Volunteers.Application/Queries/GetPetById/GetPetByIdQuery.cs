using NewPetHome.Core.Abstraction;

namespace NewPetHome.Volunteers.Application.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId):IQuery;