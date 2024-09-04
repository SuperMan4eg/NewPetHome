using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using NewPetHome.Applications.FileProvider;
using NewPetHome.Applications.Providers;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int EXPIRY = 60 * 60 * 24;
    
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadFile(
        FileData fileData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExist = IsBucketExist(fileData.FileMetaData.BucketName, cancellationToken).Result;

            if (bucketExist == false)
            {
                await CreateBucket(fileData.FileMetaData.BucketName, cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.FileMetaData.BucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.FileMetaData.ObjectName);

            var result = await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fail to upload file in minio");
            return Error.Failure("file.upload", "Fail to upload file in minio");
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