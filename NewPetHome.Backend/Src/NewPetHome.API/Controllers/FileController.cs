using Microsoft.AspNetCore.Mvc;
using NewPetHome.API.Extensions;
using NewPetHome.Applications.FileProvider;
using NewPetHome.Applications.Providers;

namespace NewPetHome.API.Controllers;

public class FileController : ApplicationController
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;

    public FileController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFile(IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();

        var filename = Guid.NewGuid().ToString();

        var fileMetaData = new FileMetaData(BUCKET_NAME, filename);

        var fileData = new FileData(stream, fileMetaData);

        var result = await _fileProvider.UploadFile(fileData, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }

        return Ok(result.Value);
    }

    [HttpDelete("{fileName:guid}")]
    public async Task<IActionResult> RemoveFile(
        [FromRoute] Guid fileName,
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(BUCKET_NAME, fileName.ToString());

        var result = await _fileProvider.DeleteFile(fileMetaData, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }

        return Ok(result.Value);
    }

    [HttpGet("{fileName:guid}")]
    public async Task<IActionResult> GetFileByName(
        [FromRoute] Guid fileName,
        CancellationToken cancellationToken)
    {
        var fileMetaData = new FileMetaData(BUCKET_NAME, fileName.ToString());

        var result = await _fileProvider.GetFileByName(fileMetaData, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }

        return Ok(result.Value);
    }
}