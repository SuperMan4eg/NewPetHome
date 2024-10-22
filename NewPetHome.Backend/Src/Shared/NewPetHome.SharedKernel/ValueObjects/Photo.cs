using CSharpFunctionalExtensions;

namespace NewPetHome.SharedKernel.ValueObjects;

public record Photo
{
    private Photo(FilePath path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public FilePath Path { get; }
    public bool IsMain { get; private set; }

    public static Result<Photo, Error> Create(FilePath path, bool isMain)
    {
        return new Photo(path, isMain);
    }

    public void SetAsMain()
    {
        IsMain = true;
    }

    public void UnsetAsMain()
    {
        IsMain = false;
    }
}