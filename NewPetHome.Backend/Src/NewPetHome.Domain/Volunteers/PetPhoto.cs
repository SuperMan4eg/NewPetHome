namespace NewPetHome.Domain.Volunteers;

public record PetPhoto
{
    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }
    
    public string Path { get; }
    public bool IsMain { get;  }

    public static PetPhoto Create(string path, bool isMain)
    {
        if(path is null)
            throw new ArgumentNullException(nameof(path));
        
        return new PetPhoto(path, isMain);
    }
}