using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetMainPhoto;

public class UpdatePetMainPhotoHandler : ICommandHandler<Guid, UpdatePetMainPhotoCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<UpdatePetMainPhotoCommand> _validator;
    private readonly ILogger<UpdatePetMainPhotoHandler> _logger;

    public UpdatePetMainPhotoHandler(
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<UpdatePetMainPhotoCommand> validator,
        ILogger<UpdatePetMainPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetMainPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petResult = volunteerResult.Value.GetPetById(PetId.Create(command.PetId));
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var filePathResult = FilePath.Create(command.FilePath);
        if (filePathResult.IsFailure)
            return filePathResult.Error.ToErrorList();

        var petPhotoResult = Photo.Create(filePathResult.Value, true);
        if (petPhotoResult.IsFailure)
        return petPhotoResult.Error.ToErrorList();

        var updateResult = volunteerResult.Value.UpdatePetMainPhoto(petResult.Value, petPhotoResult.Value);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();
        
        await _volunteersUnitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Main photo updated with pet {PetId}", command.PetId);

        return command.PetId;
    }
}