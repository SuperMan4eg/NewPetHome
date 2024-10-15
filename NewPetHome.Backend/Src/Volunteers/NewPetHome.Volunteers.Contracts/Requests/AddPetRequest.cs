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
    int Weight,
    int Height,
    string PhoneNumber,
    bool IsCastrated,
    DateTime BirthDate,
    bool IsVaccinated,
    string Status,
    IEnumerable<RequisiteDto> Requisites);