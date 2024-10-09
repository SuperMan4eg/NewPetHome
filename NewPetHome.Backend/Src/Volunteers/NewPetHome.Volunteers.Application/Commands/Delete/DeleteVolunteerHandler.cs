using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Commands.Delete;

public class DeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteVolunteerCommand> _validator;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<DeleteVolunteerCommand> validator,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.SoftDelete();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer delete with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}