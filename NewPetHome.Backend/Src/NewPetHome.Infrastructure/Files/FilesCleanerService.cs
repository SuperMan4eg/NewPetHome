using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Files;
using NewPetHome.Applications.Messaging;
using NewPetHome.Infrastructure.BackgroundServices;
using FileInfo = NewPetHome.Applications.Files.FileInfo;

namespace NewPetHome.Infrastructure.Files;

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