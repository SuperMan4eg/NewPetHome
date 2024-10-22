using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.Core.Messaging;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using FileInfo = NewPetHome.Core.FileInfo;
using IFileProvider = NewPetHome.Core.IFileProvider;

namespace NewPetHome.Volunteers.Application.Commands.UploadFilesToPet;

public class UploadFilesToPetHandler : ICommandHandler<Guid, UploadFilesToPetCommand>
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IVolunteersUnitOfWork _volunteersUnitOfWork;
    private readonly IValidator<UploadFilesToPetCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly ILogger<UploadFilesToPetHandler> _logger;

    public UploadFilesToPetHandler(
        IFileProvider fileProvider,
        IVolunteersRepository volunteersRepository,
        IVolunteersUnitOfWork volunteersUnitOfWork,
        IValidator<UploadFilesToPetCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        ILogger<UploadFilesToPetHandler> logger)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
        _volunteersUnitOfWork = volunteersUnitOfWork;
        _validator = validator;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UploadFilesToPetCommand command,
        CancellationToken cancellationToken = default)
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

        List<FileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileInfo = new FileInfo(filePath.Value, BUCKET_NAME);
            var fileData = new FileData(file.Content, fileInfo);

            filesData.Add(fileData);
        }

        var filePathsResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
        if (filePathsResult.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.Info), cancellationToken);

            return filePathsResult.Error.ToErrorList();
        }

        var petPhotos = filePathsResult.Value
            .Select(f => Photo.Create(f, false).Value)
            .ToList();

        volunteerResult.Value.AddPetPhotos(petId, petPhotos);

        await _volunteersUnitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Success uploaded photos to pet - {petId}", petId.Value);

        return petResult.Value.Id.Value;
    }
}