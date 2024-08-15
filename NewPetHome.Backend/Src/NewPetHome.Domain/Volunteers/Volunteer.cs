using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }
    
    public string FullName { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Experience { get; private set; }
    public int CountPetsFindHome { get; private set; }
    public int CountPetsLookingHome { get; private set; }
    public int CountPetsInTreatment { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<Pet> Pets => _pets;
    public VolunteerDetails Details { get; private set; } = default!;

}