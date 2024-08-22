using CSharpFunctionalExtensions;

namespace NewPetHome.Domain.Shared.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)|| value.Length > Constants.MAX_PHONE_NUMBER_LENGTH)
            return Errors.General.ValueIsInvalid("phone number");

        return new PhoneNumber(value);
    }
}