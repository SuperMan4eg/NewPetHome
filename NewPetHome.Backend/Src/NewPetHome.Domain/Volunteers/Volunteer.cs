using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Volunteers;

public sealed class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }

    private Volunteer(
        VolunteerId id, FullName fullName, int experience, string description, string phoneNumber,
        VolunteerDetails details) :
        base(id)
    {
        FullName = fullName;
        Experience = experience;
        Description = description;
        PhoneNumber = phoneNumber;
        Details = details;
    }

    public FullName FullName { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Experience { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<Pet> Pets => _pets;
    public VolunteerDetails Details { get; private set; } = default!;

    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        FullName fullName,
        int experience,
        string description,
        string phoneNumber,
        VolunteerDetails details)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("description");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Errors.General.ValueIsInvalid("phoneNember");

        if (experience < 0)
            return Errors.General.ValueIsInvalid("experience");

        var volunteer = new Volunteer(id, fullName, experience, description, phoneNumber, details);

        return volunteer;
    }

    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
    }

    public int CountPetsFindHome() => _pets.Count(p => p.Status == PetStatus.FindHome);

    public int CountPetsLookingHome() => _pets.Count(p => p.Status == PetStatus.LookingHome);

    public int CountPetsInTreatment() => _pets.Count(p => p.Status == PetStatus.InTreatment);
}