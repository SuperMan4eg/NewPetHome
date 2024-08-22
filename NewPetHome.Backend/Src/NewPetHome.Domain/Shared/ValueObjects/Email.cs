using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace NewPetHome.Domain.Shared.ValueObjects;

public record Email
{
    private const string EMAIL_REGEX = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    
    private Email(string value) => Value = value;
    
    public string Value { get; } = default!;

    public static Result<Email, Error> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, EMAIL_REGEX))
        {
            return Errors.General.ValueIsInvalid("email");
        }

        return new Email(email);
    }
}