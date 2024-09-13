using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.FileProvider;

public interface IFileProvider
{
    public Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);

    public Task<Result<string, Error>> DeleteFile(
        FileMetaData fileData, CancellationToken cancellationToken = default);

    public Task<Result<string, Error>> GetFileByName(
        FileMetaData fileData, CancellationToken cancellationToken = default);
}