using Microsoft.AspNetCore.Mvc;
using NewPetHome.API.Extensions;
using NewPetHome.Applications.Files;

namespace NewPetHome.API.Controllers;

public class FileController : ApplicationController
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;

    public FileController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
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