using NewPetHome.Domain.Shared;

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
        
    public static Result<SocialNetwork> Create(string name, string url)
    {
        if(string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return $"Social network name can not be empty or more than {Constants.MAX_LOW_TEXT_LENGTH} characters";
        
        if(string.IsNullOrWhiteSpace(url) || url.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"Social network url can not be empty or more than {Constants.MAX_HIGH_TEXT_LENGTH} characters";
        
        return new SocialNetwork(name, url);
    }
}