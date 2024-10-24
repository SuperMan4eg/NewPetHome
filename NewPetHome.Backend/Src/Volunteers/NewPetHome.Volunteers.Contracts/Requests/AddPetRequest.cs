using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Contracts.Requests;

public record AddPetRequest(
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
    string Status,
    IEnumerable<RequisiteDto> Requisites);