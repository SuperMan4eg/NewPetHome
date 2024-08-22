using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record SocialNetworks
{
    private SocialNetworks()
    {
    }

    public SocialNetworks(IEnumerable<SocialNetwork> socials)
    {
        Socials = socials.ToList();
    }

    public IReadOnlyList<SocialNetwork> Socials { get; } = [];
}