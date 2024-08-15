namespace NewPetHome.Domain.Volunteers;

public record SocialNetwork
{
    private SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }
    
    public string Name { get;}
    public string Url { get;}
        
    public static SocialNetwork Create(string name, string url)
    {
        if(name is null)
            throw new ArgumentNullException(nameof(name));
        
        if(url is null)
            throw new ArgumentNullException(nameof(url));
        
        return new SocialNetwork(name, url);
    }
}