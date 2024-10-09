using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Domain.ValueObjects;

public class Experience : ValueObject
{
    private Experience(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<Experience, Error> Create(int value)
    {
        if (value < 0 || value > 100)
            return Errors.General.ValueIsInvalid("experience");

        return new Experience(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}