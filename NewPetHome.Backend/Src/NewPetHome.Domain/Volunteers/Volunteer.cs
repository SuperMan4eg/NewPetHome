using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }

    private Volunteer(
        VolunteerId id, string fullName, int experience, string description, string phoneNumber,
        List<Requisite> requisites = default!, List<SocialNetwork> socialNetworks = default!) :
        base(id)
    {
        FullName = fullName;
        Experience = experience;
        Description = description;
        PhoneNumber = phoneNumber;
        Details = VolunteerDetails.Create(requisites, socialNetworks);
    }

    public string FullName { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Experience { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<Pet> Pets => _pets;
    public VolunteerDetails Details { get; private set; } = default!;

    public static Volunteer Create(VolunteerId id, string fullName, int experience, string description,
        string phoneNumber, List<Requisite> requisites = default!, List<SocialNetwork> socialNetworks = default!)
    {
        return new(
            id,
            fullName,
            experience,
            description,
            phoneNumber,
            requisites,
            socialNetworks
        );
    }

    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
    }

    public int CountPetsFindHome()
    {
        var result = _pets.Count(p => p.Status == PetStatus.FindHome);
        return result;
    }

    public int CountPetsLookingHome()
    {
        var result = _pets.Count(p => p.Status == PetStatus.LookingHome);
        return result;
    }

    public int CountPetsInTreatment()
    {
        var result = _pets.Count(p => p.Status == PetStatus.InTreatment);
        return result;
    }
}