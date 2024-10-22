using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Contracts.Requests;

public record UpdatePetInfoRequest(
    string Name,
    string Description,
    Guid SpecieId,
    Guid BreedId,
    string Color,
    string HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsCastrated,
    DateTime BirthDate,
    bool IsVaccinated,
    IEnumerable<RequisiteDto> Requisites);