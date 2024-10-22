using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Contracts;
using NewPetHome.Volunteers.Domain.Enums;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetInfo;

public class UpdatePetInfoHandler : ICommandHandler<Guid, UpdatePetInfoCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly ISpeciesContract _speciesContract;
    private readonly IValidator<UpdatePetInfoCommand> _validator;
    private readonly ILogger<UpdatePetInfoHandler> _logger;

    public UpdatePetInfoHandler(
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        ISpeciesContract speciesContract,
        IValidator<UpdatePetInfoCommand> validator,
        ILogger<UpdatePetInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _speciesContract = speciesContract;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetInfoCommand command,
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

        var petExistResult = volunteerResult.Value.IsPetExist(command.PetId);
        if(petExistResult.IsFailure)
            return petExistResult.Error.ToErrorList();

        var name = Name.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        var typeDetails = TypeDetails.Create(specieResult.Value.Id, breed.Id).Value;
        var color = Color.Create(command.Color).Value;
        var healthInfo = HealthInfo.Create(command.HealthInfo).Value;
        var address = Address.Create(command.Address.City, command.Address.Street, command.Address.HouseNumber).Value;
        var weight = Weight.Create(command.Weight).Value;
        var height = Height.Create(command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var requisites = new List<Requisite>(command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));

        volunteerResult.Value.UpdatePetInfo(
            command.PetId,
            name,
            description,
            typeDetails,
            color,
            healthInfo,
            address,
            weight,
            height,
            phoneNumber,
            command.IsCastrated,
            command.BirthDate,
            command.IsVaccinated,
            requisites);

        await _volunteersUnitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet info updated with id: {PetId}.", command.PetId);

        return command.PetId;
    }
}