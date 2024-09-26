using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.Files;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData, CancellationToken cancellationToken = default);

    Task<Result<string, Error>> DeleteFile(
        FileMetaData fileData, CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveFile(
        FileInfo fileInfo, CancellationToken cancellationToken = default);

    Task<Result<string, Error>> GetFileByName(
        FileMetaData fileData, CancellationToken cancellationToken = default);
}