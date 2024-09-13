using CSharpFunctionalExtensions;

namespace NewPetHome.Domain.Shared.ValueObjects;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("name");

        return new Name(value);
    }
}