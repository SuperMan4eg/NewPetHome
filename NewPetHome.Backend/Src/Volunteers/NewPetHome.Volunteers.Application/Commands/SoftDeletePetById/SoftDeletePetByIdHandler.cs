using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Commands.SoftDeletePetById;

public class SoftDeletePetByIdHandler : ICommandHandler<Guid, SoftDeletePetByIdCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<SoftDeletePetByIdCommand> _validator;
    private readonly ILogger<SoftDeletePetByIdHandler> _logger;

    public SoftDeletePetByIdHandler(
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<SoftDeletePetByIdCommand> validator,
        ILogger<SoftDeletePetByIdHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeletePetByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        volunteerResult.Value.SoftDeletePet(petResult.Value.Id);

        await _volunteersUnitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet was soft deleted with id: {PetId}.", petResult.Value.Id);

        return petResult.Value.Id.Value;
    }
}