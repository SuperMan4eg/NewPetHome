using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Extensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.VolunteersManagement.Commands.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteer = InitVolunteer(command);

        await _volunteersRepository.Add(volunteer, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer created with id: {VolunteerId}.", volunteer.Id.Value);

        return volunteer.Id.Value;
    }

    private Volunteer InitVolunteer(CreateVolunteerCommand command)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName).Value;
        var description = Description.Create(command.Description).Value;
        var email = Email.Create(command.Email).Value;
        var experience = Experience.Create(command.Experience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var requisites = new ValueObjectList<Requisite>(command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));
        var socialNetworks = new ValueObjectList<SocialNetwork>(command.SocialNetworks.Select(r =>
            SocialNetwork.Create(r.Name, r.Url).Value));

        return new Volunteer(
            volunteerId,
            fullName,
            description,
            email,
            experience,
            phoneNumber,
            requisites,
            socialNetworks);
    }
}