namespace NewPetHome.Domain;

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
    
    public static Address Create(string city, string street, int houseNumber)
    {
        if(string.IsNullOrWhiteSpace(city))
            throw new ArgumentNullException(nameof(city));
        
        if(string.IsNullOrWhiteSpace(street))
            throw new ArgumentNullException(nameof(street));
        
        return new Address(city, street, houseNumber);
    }
}