namespace NewPetHome.Domain.Volunteers;

public record VolunteerDetails
{
    private VolunteerDetails()
    {
    }

    public VolunteerDetails(IEnumerable<Requisite> requisites, IEnumerable<SocialNetwork> socialNetworks)
    {
        Requisites = requisites.ToList();
        SocialNetworks = socialNetworks.ToList();
    }

    public IReadOnlyList<Requisite> Requisites { get; }
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
}