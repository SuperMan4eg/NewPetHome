using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.Entitys;

public sealed class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = [];

    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description description,
        Email email,
        Experience experience,
        PhoneNumber phoneNumber,
        RequisitesList requisites,
        SocialNetworks socialNetworks) :
        base(id)
    {
        FullName = fullName;
        Description = description;
        Email = email;
        Experience = experience;
        PhoneNumber = phoneNumber;
        Requisites = requisites;
        SocialNetworks = socialNetworks;
    }

    public FullName FullName { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Experience Experience { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<Pet> Pets => _pets;
    public RequisitesList Requisites { get; private set; } = default!;
    public SocialNetworks SocialNetworks { get; private set; } = default!;

    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
    }

    public int CountPetsFindHome() => _pets.Count(p => p.Status == PetStatus.FindHome);

    public int CountPetsLookingHome() => _pets.Count(p => p.Status == PetStatus.LookingHome);

    public int CountPetsInTreatment() => _pets.Count(p => p.Status == PetStatus.InTreatment);
}