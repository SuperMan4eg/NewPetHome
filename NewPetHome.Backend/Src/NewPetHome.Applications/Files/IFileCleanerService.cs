namespace NewPetHome.Applications.Files;

public interface IFileCleanerService
{
    Task Process(CancellationToken cancellationToken);
}