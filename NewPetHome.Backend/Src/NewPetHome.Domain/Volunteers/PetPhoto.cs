using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Volunteers;

public record PetPhoto
{
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public string Path { get; }
    public bool IsMain { get; }

    public static Result<PetPhoto> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "Photo path can not be empty";
        }

        return new PetPhoto(path, isMain);
    }
}