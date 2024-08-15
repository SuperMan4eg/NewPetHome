namespace NewPetHome.Domain.Volunteers;

public record Requisites
{
    private Requisites(string name, string description)
    {
        Name = name;
        Description = description;
    }
    
    public string Name { get; }
    public string Description { get; }
    
    public static Requisites Create(string name, string description)
    {
        if(name is null)
            throw new ArgumentNullException(nameof(name));
        
        if(description is null)
            throw new ArgumentNullException(nameof(description));
        
        return new Requisites(name, description);
    }
}