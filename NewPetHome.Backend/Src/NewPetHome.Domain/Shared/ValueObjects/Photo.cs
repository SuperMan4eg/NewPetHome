using CSharpFunctionalExtensions;

namespace NewPetHome.Domain.Shared.ValueObjects;

public record Photo
{
    private Photo(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public string Path { get; }
    public bool IsMain { get; }

    public static Result<Photo, Error> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Errors.General.ValueIsInvalid("path");
        }

        return new Photo(path, isMain);
    }
}