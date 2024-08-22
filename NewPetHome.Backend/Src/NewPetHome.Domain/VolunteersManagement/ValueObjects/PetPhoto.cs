using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record PetPhoto
{
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public string Path { get; }
    public bool IsMain { get; }

    public static Result<PetPhoto, Error> Create(string path, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Errors.General.ValueIsInvalid("path");
        }

        return new PetPhoto(path, isMain);
    }
}