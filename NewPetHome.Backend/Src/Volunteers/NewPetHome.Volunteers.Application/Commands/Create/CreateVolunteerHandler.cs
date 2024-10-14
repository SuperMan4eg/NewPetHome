using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Volunteers.Domain.Entities;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
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

        await _volunteersUnitOfWork.SaveChanges(cancellationToken);

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