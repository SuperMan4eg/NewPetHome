using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Domain.ValueObjects;

public class HealthInfo : ValueObject
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}