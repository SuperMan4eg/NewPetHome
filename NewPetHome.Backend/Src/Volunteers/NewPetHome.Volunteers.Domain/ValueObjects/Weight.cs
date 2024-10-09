using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Domain.ValueObjects;

public class Weight : ValueObject
{
    private Weight(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Result<Weight, Error> Create(double value)
    {
        if (value < 0 || value > 100)
            return Errors.General.ValueIsInvalid("weight");

        return new Weight(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}