namespace NewPetHome.Domain.Volunteers;

public record VolunteerDetails
{
    private readonly List<Requisite> _requisites;
    private readonly List<SocialNetwork> _socialNetworks;

    protected VolunteerDetails()
    {
    }

    private VolunteerDetails(List<Requisite> requisites, List<SocialNetwork> socialNetworks)
    {
        _requisites = requisites ??= [];
        _socialNetworks = socialNetworks ??= [];
    }

    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    
    public static VolunteerDetails Create(List<Requisite> requisites, List<SocialNetwork> socialNetworks)
    {
        return new(requisites, socialNetworks);
    }
}