using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.VolunteersManagement.Commands.AddPet;

namespace NewPetHome.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
    string Name,
    string Description,
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
    IEnumerable<RequisiteDto> Requisites)
{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId,
            Name,
            Description,
            Color,
            HealthInfo,
            Address,
            Weight,
            Height,
            PhoneNumber,
            IsCastrated,
            BirthDate,
            IsVaccinated,
            Status,
            Requisites);
}