using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record Weight
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
}