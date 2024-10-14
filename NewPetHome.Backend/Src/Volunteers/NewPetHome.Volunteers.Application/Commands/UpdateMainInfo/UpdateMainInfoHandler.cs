using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.UpdateMainInfo;

public class UpdateMainInfoHandler : ICommandHandler<Guid, UpdateMainInfoCommand>
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<UpdateMainInfoCommand> _validator;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<UpdateMainInfoCommand> validator,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName).Value;
        var description = Description.Create(command.Description).Value;
        var email = Email.Create(command.Email).Value;
        var experience = Experience.Create(command.Experience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            description,
            email,
            experience,
            phoneNumber);

        await _volunteersUnitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer main info update with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}