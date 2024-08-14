using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain;

public class Volunteer : Entity
{
    private readonly List<Requisites> _requisites = [];
    private readonly List<Pet> _pets = [];
    private readonly List<SocialNetwork> _socialNetworks = [];

    private Volunteer(Guid id) : base(id)
    {
    }
    
    public string FullName { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Experience { get; private set; }
    public int CountPetsFindHome { get; private set; }
    public int CountPetsLookingHome { get; private set; }
    public int CountPetsInTreatment { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public IReadOnlyList<Pet> Pets => _pets;

}