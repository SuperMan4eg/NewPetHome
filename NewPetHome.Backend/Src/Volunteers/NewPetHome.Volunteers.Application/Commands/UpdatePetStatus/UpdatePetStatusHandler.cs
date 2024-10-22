using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Volunteers.Domain.Enums;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<UpdatePetStatusCommand> _validator;
    private readonly ILogger<UpdatePetStatusHandler> _logger;

    public UpdatePetStatusHandler(
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<UpdatePetStatusCommand> validator,
        ILogger<UpdatePetStatusHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _validator = validator;
        _logger = logger;
    }
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petExistResult = volunteerResult.Value.IsPetExist(command.PetId);
        if(petExistResult.IsFailure)
            return petExistResult.Error.ToErrorList();

        var status = Enum.Parse<PetStatus>(command.Status);
        
        volunteerResult.Value.UpdatePetStatus(command.PetId, status);

        await _volunteersUnitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Success update pet status from pet - {PetId}", command.PetId);
        
        return command.PetId;
    }
}