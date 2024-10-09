using CSharpFunctionalExtensions;

namespace NewPetHome.SharedKernel.ValueObjects;

public record Address
{
    private Address(string city, string street, int houseNumber)
    {
        City = city;
        Street = street;
        HouseNumber = houseNumber;
    }

    public string City { get; }
    public string Street { get; }
    public int HouseNumber { get; }

    public static Result<Address, Error> Create(string city, string street, int houseNumber)
    {
        if (string.IsNullOrWhiteSpace(city) || city.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("city");

        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("street");

        if (houseNumber < 0)
            return Errors.General.ValueIsInvalid("house number");

        return new Address(city, street, houseNumber);
    }
}