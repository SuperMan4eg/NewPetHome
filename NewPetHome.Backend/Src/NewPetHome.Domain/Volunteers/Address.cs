using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Volunteers;

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
    
    public static Result<Address> Create(string city, string street, int houseNumber)
    {
        if(string.IsNullOrWhiteSpace(city))
            return "City can not be empty";
        
        if(string.IsNullOrWhiteSpace(street))
            return "Street can not be empty";
        
        return new Address(city, street, houseNumber);
    }
}