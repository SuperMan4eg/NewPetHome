namespace NewPetHome.Volunteers.Application.Files;

public interface IFileCleanerService
{
    Task Process(CancellationToken cancellationToken);
}