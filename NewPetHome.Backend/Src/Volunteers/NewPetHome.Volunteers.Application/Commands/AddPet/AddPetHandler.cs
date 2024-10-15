using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Contracts;
using NewPetHome.Volunteers.Domain.Entities;
using NewPetHome.Volunteers.Domain.Enums;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly ISpeciesContract _speciesContract;
    private readonly IValidator<AddPetCommand> _validator;
    private readonly ILogger<AddPetHandler> _logger;

    public AddPetHandler(
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        ISpeciesContract speciesContract,
        IValidator<AddPetCommand> validator,
        ILogger<AddPetHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _speciesContract = speciesContract;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var specieResult = await _speciesContract.GetSpecieById(command.SpecieId, cancellationToken);
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();


        var breed = specieResult.Value.Breeds.FirstOrDefault(b => b.Id == command.BreedId);
        if (breed is null)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        var pet = InitPet(command);
        volunteerResult.Value.AddPet(pet);

        await _volunteersUnitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet added with id: {PetId}.", pet.Id.Value);

        return pet.Id.Value;
    }

    private Pet InitPet(AddPetCommand command)
    {
        var petId = PetId.NewPetId();
        var name = Name.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        var specieId = SpecieId.Create(command.SpecieId).Value;
        var breedId = BreedId.Create(command.BreedId).Value;
        var typeDetails = TypeDetails.Create(specieId, breedId).Value;
        var color = Color.Create(command.Color).Value;
        var healthInfo = HealthInfo.Create(command.HealthInfo).Value;
        var address = Address.Create(command.Address.City, command.Address.Street, command.Address.HouseNumber).Value;
        var weight = Weight.Create(command.Weight).Value;
        var height = Height.Create(command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var isCastrated = command.IsCastrated;
        var birthDate = command.BirthDate.ToUniversalTime();
        var isVaccinated = command.IsVaccinated;
        var status = Enum.Parse<PetStatus>(command.Status);
        var requisites = new List<Requisite>(command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));

        return new Pet(
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
            requisites);
    }
}