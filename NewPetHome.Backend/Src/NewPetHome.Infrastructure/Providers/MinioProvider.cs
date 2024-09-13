using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using NewPetHome.Applications.FileProvider;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_DEGREE_OF_PARALLELISM = 10;
    private const int EXPIRY = 60 * 60 * 24;

    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
        var filesList = filesData.ToList();

        try
        {
            await IfBucketsNotExistCreateBuckets(filesList, cancellationToken);

            var tasks = filesList.Select(async file =>
                await PutObject(file, semaphoreSlim, cancellationToken));

            var pathResult = await Task.WhenAll(tasks);

            if (pathResult.Any(p => p.IsFailure))
                return pathResult.First().Error;

            var results = pathResult.Select(p => p.Value).ToList();

            _logger.LogInformation("Uploaded files {files}", results.Select(f => f.Path));

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload files in minio, files amount: {amount}", filesList.Count);

            return Error.Failure("file.upload", "Fail to upload files in minio");
        }
    }

    public async Task<Result<string, Error>> DeleteFile(
        FileMetaData fileMetaData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = IsBucketExist(fileMetaData.BucketName, cancellationToken).Result;

            if (bucketExist == false)
                return Error.NotFound("bucket.not.found", "Bucket not found");

            var objectExistArgs = new PresignedGetObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName)
                .WithExpiry(EXPIRY);

            var objectExist = await _minioClient.PresignedGetObjectAsync(objectExistArgs);

            if (string.IsNullOrWhiteSpace(objectExist))
            {
                return Error.NotFound("file.not.found", "File not found");
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

            return fileMetaData.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to delete file in minio");
            return Error.Failure("file.delete", "Fail to delete file in minio");
        }
    }

    public async Task<Result<string, Error>> GetFileByName(
        FileMetaData fileMetaData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = IsBucketExist(fileMetaData.BucketName, cancellationToken).Result;

            if (bucketExist == false)
                return Error.NotFound("bucket.not.found", "Bucket not found");

            var objectExistArgs = new PresignedGetObjectArgs()
                .WithBucket(fileMetaData.BucketName)
                .WithObject(fileMetaData.ObjectName)
                .WithExpiry(EXPIRY);

            var objectUrl = await _minioClient.PresignedGetObjectAsync(objectExistArgs);

            if (string.IsNullOrWhiteSpace(objectUrl))
            {
                return Error.NotFound("file.not.found", "File not found");
            }

            return objectUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to download file in minio");
            return Error.Failure("file.download", "Fail to download file in minio");
        }
    }

    private async Task<Result<FilePath, Error>> PutObject(
        FileData fileData,
        SemaphoreSlim semaphoreSlim,
        CancellationToken cancellationToken = default)
    {
        await semaphoreSlim.WaitAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(fileData.BucketName)
            .WithStreamData(fileData.Stream)
            .WithObjectSize(fileData.Stream.Length)
            .WithObject(fileData.FilePath.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, cancellationToken);

            return fileData.FilePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with path {path} in bucket {bucket}",
                fileData.FilePath.Path,
                fileData.BucketName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task IfBucketsNotExistCreateBuckets(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default)
    {
        HashSet<String> bucketNames = [..filesData.Select(file => file.BucketName)];

        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient
                .BucketExistsAsync(bucketExistArgs, cancellationToken);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient
                    .MakeBucketAsync(makeBucketArgs, cancellationToken);
            }
        }
    }

    private async Task<bool> IsBucketExist(string bucketName, CancellationToken cancellationToken = default)
    {
        var bucketExistArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, cancellationToken);

        return bucketExist;
    }

    private async Task CreateBucket(string bucketName, CancellationToken cancellationToken = default)
    {
        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(bucketName);

        await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
    }
}