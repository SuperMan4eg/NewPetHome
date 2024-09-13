using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record HealthInfo
{
    private HealthInfo(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<HealthInfo, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("health info");

        return new HealthInfo(value);
    }
}