using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Extensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.VolunteersManagement.Commands.UpdateRequisites;

public class UpdateRequisitesHandler : ICommandHandler<Guid, UpdateRequisitesCommand>
{
    private readonly ILogger<UpdateRequisitesHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateRequisitesCommand> _validator;

    public UpdateRequisitesHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateRequisitesCommand> validator,
        ILogger<UpdateRequisitesHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var requisites = new ValueObjectList<Requisite>(command.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));

        volunteerResult.Value.UpdateRequisites(requisites);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer requisites update with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}