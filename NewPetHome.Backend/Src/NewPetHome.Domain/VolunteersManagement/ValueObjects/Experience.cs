using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record Experience
{
    private Experience(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<Experience, Error> Create(int value)
    {
        if (value < 0|| value > 100)
            return Errors.General.ValueIsInvalid("experience");

        return new Experience(value);
    }
}