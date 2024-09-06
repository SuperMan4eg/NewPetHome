using CSharpFunctionalExtensions;
using NewPetHome.Applications.Providers;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.Volunteers.AddPet;

public class AddPetHandler
{
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetHandler(
        IFileProvider fileProvider,
        IVolunteersRepository volunteersRepository)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(
            VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var petId = PetId.NewPetId();
        var name = Name.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        var typeDetails = TypeDetails.Create(SpeciesId.Empty(), BreedId.Empty()).Value;
        var color = Color.Create(command.Color).Value;
        var healthInfo = HealthInfo.Create(command.HealthInfo).Value;
        var address = Address.Create(command.Address.City, command.Address.Street, command.Address.HouseNumber).Value;
        var weight = Weight.Create(command.Weight).Value;
        var height = Height.Create(command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var isCastrated = command.IsCastrated;
        var birthDate = command.BirthDate;
        var isVaccinated = command.IsVaccinated;
        var status = Enum.Parse<PetStatus>(command.Status);
        var requisites = new RequisitesList(command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));
        var photos = command.Files
            .Select(f => Photo.Create(f.FileName, false).Value);

        var pet = new Pet(
            petId,
            name,
            description,
            typeDetails,
            color,
            healthInfo,
            address,
            weight,
            height,
            phoneNumber,
            isCastrated,
            birthDate,
            isVaccinated,
            status,
            new PetPhotos(photos),
            requisites);

        volunteerResult.Value.AddPet(pet);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        return petId.Value;
    }
}