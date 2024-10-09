using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewPetHome.Volunteers.Application.Files;

namespace NewPetHome.Volunteers.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundServices : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundServices> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public FilesCleanerBackgroundServices(
        ILogger<FilesCleanerBackgroundServices> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Files cleaner background service started.");

        await using var scope = _scopeFactory.CreateAsyncScope();

        var filesCleanerService = scope.ServiceProvider.GetRequiredService<IFileCleanerService>();

        while (!cancellationToken.IsCancellationRequested)
        {
            await filesCleanerService.Process(cancellationToken);
        }

        await Task.CompletedTask;
    }
}