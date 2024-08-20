using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Volunteers;

public record FullName
{
    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    
    public string FirstName { get; }
    public string LastName { get; }

    public static Result<FullName> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return $"First name can not be empty or more than {Constants.MAX_LOW_TEXT_LENGTH} characters";

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return $"Last name can not be empty or more than {Constants.MAX_LOW_TEXT_LENGTH} characters";
        
        return new FullName(firstName, lastName);
    }
}
