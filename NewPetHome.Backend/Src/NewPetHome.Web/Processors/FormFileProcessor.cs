using NewPetHome.Core.Dtos;
using NewPetHome.Volunteers.Contracts.Converters;

namespace NewPetHome.Web.Processors;

public class FormFileConverter : IFormFileConverter
{
    private readonly List<Stream> _fileStreams = [];

    public IEnumerable<UploadFileDto> ToUploadFileDtos(IFormFileCollection files)
    {
        List<UploadFileDto> fileDtos = [];

        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new UploadFileDto(stream, file.FileName);
            fileDtos.Add(fileDto);
            _fileStreams.Add(fileDto.Content);
        }

        return fileDtos;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var item in _fileStreams)
        {
            await item.DisposeAsync();
        }
    }
}