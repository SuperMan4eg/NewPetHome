using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using FileInfo = NewPetHome.Core.FileInfo;

namespace NewPetHome.Volunteers.Application.Commands.HardDeletePetById;

public class HardDeletePetByIdHandler : ICommandHandler<Guid, HardDeletePetByIdCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<HardDeletePetByIdCommand> _validator;
    private readonly ILogger<HardDeletePetByIdHandler> _logger;

    public HardDeletePetByIdHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<HardDeletePetByIdCommand> validator,
        ILogger<HardDeletePetByIdHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        HardDeletePetByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _volunteersUnitOfWork.BeginTransaction(cancellationToken);
        try
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

            var petFileInfos = petResult.Value.Photos
                .Select(p => new FileInfo(p.Path, Constants.BUCKET_NAME)).ToList();

            volunteerResult.Value.HardDeletePet(petResult.Value);

            await _volunteersUnitOfWork.SaveChanges(cancellationToken);

            foreach (var fileInfo in petFileInfos)
            {
                var deleteFileResult = await _fileProvider.RemoveFile(fileInfo, cancellationToken);
                if (deleteFileResult.IsFailure)
                    return deleteFileResult.Error.ToErrorList();
            }

            transaction.Commit();

            _logger.LogInformation("Pet was hard deleted with id: {PetId}.", petResult.Value.Id);

            return petResult.Value.Id.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete pet with id - {petId}", command.PetId);

            transaction.Rollback();

            return Error.Failure("pet.delete", "Fail to delete pet")
                .ToErrorList();
        }
    }
}