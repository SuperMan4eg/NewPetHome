using CSharpFunctionalExtensions;

namespace NewPetHome.Domain.Shared.ValueObjects;

public record Photo
{
    private Photo(FilePath path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public FilePath Path { get; }
    public bool IsMain { get; }

    public static Result<Photo, Error> Create(FilePath path, bool isMain)
    {
        return new Photo(path, isMain);
    }
}