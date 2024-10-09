using Microsoft.AspNetCore.Http;
using NewPetHome.Core.Dtos;

namespace NewPetHome.Volunteers.Contracts.Converters;

public interface IFormFileConverter : IAsyncDisposable
{
    IEnumerable<UploadFileDto> ToUploadFileDtos(IFormFileCollection formFiles);
}