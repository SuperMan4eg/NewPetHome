namespace NewPetHome.Domain.Volunteers;

public record VolunteerDetails
{
    private readonly List<Requisites> _requisites = [];
    private readonly List<SocialNetwork> _socialNetworks = [];
    
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
}