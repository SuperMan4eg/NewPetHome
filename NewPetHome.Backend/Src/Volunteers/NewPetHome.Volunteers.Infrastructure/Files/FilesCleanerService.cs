using Microsoft.Extensions.Logging;
using NewPetHome.Core;
using NewPetHome.Core.Messaging;
using NewPetHome.Volunteers.Application.Files;
using NewPetHome.Volunteers.Infrastructure.BackgroundServices;
using FileInfo = NewPetHome.Core.FileInfo;

namespace NewPetHome.Volunteers.Infrastructure.Files;

public class FilesCleanerService : IFileCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly ILogger<FilesCleanerBackgroundServices> _logger;

    public FilesCleanerService(
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        ILogger<FilesCleanerBackgroundServices> logger)
    {
        _fileProvider = fileProvider;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task Process(CancellationToken cancellationToken)
    {
        var fileInfos = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var fileInfo in fileInfos)
        {
            await _fileProvider.RemoveFile(fileInfo, cancellationToken);

            _logger.LogInformation("File {FileName} was removed.", fileInfo.FilePath);
        }
    }
}