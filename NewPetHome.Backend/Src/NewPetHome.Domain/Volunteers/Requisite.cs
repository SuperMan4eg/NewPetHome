using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Volunteers;

public record Requisite
{
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }
    public string Description { get; }

    public static Result<Requisite> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return $"Requisite name can not be empty or more than {Constants.MAX_LOW_TEXT_LENGTH} characters";


        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"Requisite description can not be empty or more than {Constants.MAX_HIGH_TEXT_LENGTH} characters";

        return new Requisite(name, description);
    }
}