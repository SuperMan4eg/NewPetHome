using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using FileInfo = NewPetHome.Core.FileInfo;

namespace NewPetHome.Volunteers.Application.Commands.DeletePetFiles;

public class DeletePetFilesHandler : ICommandHandler<Guid, DeletePetFilesCommand>
{
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<DeletePetFilesCommand> _validator;
    private readonly ILogger<DeletePetFilesHandler> _logger;

    public DeletePetFilesHandler(
        IFileProvider fileProvider,
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<DeletePetFilesCommand> validator,
        ILogger<DeletePetFilesHandler> logger)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeletePetFilesCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _volunteersUnitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToErrorList();

            var volunteerResult = await _volunteersRepository
                .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            List<Photo> petPhotos = [];

            foreach (var path in command.FilePaths)
            {
                var filePath = FilePath.Create(path);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var petPhoto = Photo.Create(filePath.Value, false);
                if (petPhoto.IsFailure)
                    return petPhoto.Error.ToErrorList();

                petPhotos.Add(petPhoto.Value);
            }

            foreach (var fileInfo in petPhotos.Select(photo => new FileInfo(photo.Path, Constants.BUCKET_NAME)))
            {
                var deleteFileResult = await _fileProvider.RemoveFile(fileInfo, cancellationToken);
                if (deleteFileResult.IsFailure)
                    return deleteFileResult.Error.ToErrorList();
            }

            volunteerResult.Value.DeletePetPhotos(petId, petPhotos);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            await _volunteersUnitOfWork.SaveChanges(cancellationToken);

            transaction.Commit();

            _logger.LogInformation("Success delete photos from pet - {petId}", command.PetId);

            return command.PetId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete photos from pet- {petId}", command.PetId);

            transaction.Rollback();

            return Error.Failure("pet.photos.delete", "Fail to delete photos from pet")
                .ToErrorList();
        }
    }
}