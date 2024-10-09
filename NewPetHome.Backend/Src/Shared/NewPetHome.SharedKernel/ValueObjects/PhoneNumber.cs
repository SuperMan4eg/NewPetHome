using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace NewPetHome.SharedKernel.ValueObjects;

public record PhoneNumber
{
    private const string PHONE_REGEX = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)|| Regex.IsMatch(value, PHONE_REGEX) == false)
            return Errors.General.ValueIsInvalid("phone number");

        return new PhoneNumber(value);
    }
}