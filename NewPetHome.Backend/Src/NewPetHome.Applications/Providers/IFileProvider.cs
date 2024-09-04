using CSharpFunctionalExtensions;
using NewPetHome.Applications.FileProvider;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Applications.Providers;

public interface IFileProvider
{
    public Task<Result<string, Error>> UploadFile(FileData fileData, CancellationToken cancellationToken = default);
    
    public Task<Result<string, Error>> DeleteFile(FileMetaData fileData, CancellationToken cancellationToken = default);
    
    public Task<Result<string, Error>> GetFileByName(FileMetaData fileData, CancellationToken cancellationToken = default);
}