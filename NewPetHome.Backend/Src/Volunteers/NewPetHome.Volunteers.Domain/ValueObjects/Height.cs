using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Domain.ValueObjects;

public class Height : ValueObject
{
    private Height(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Result<Height, Error> Create(double value)
    {
        if (value < 0 || value > 100)
            return Errors.General.ValueIsInvalid("height");

        return new Height(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}