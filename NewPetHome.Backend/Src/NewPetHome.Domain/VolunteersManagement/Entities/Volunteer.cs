using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.Entities;

public sealed class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;

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
        ValueObjectList<Requisite> requisites,
        ValueObjectList<SocialNetwork> socialNetworks) :
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
    public ValueObjectList<Requisite> Requisites { get; private set; }
    public ValueObjectList<SocialNetwork> SocialNetworks { get; private set; }

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if(pet is null)
            return Errors.General.NotFound(petId.Value);
        
        return pet;
    }

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        Email email,
        Experience experience,
        PhoneNumber phoneNumber
    )
    {
        FullName = fullName;
        Description = description;
        Email = email;
        Experience = experience;
        PhoneNumber = phoneNumber;
    }

    public void UpdateRequisites(ValueObjectList<Requisite> requisites)
    {
        Requisites = requisites;
    }

    public void UpdateSocialNetworks(ValueObjectList<SocialNetwork> socialNetworks)
    {
        SocialNetworks = socialNetworks;
    }

    public void Delete()
    {
        if (_isDeleted)
            return;

        _isDeleted = true;
        foreach (var pet in _pets)
            pet.Delete();
    }

    public void Restore()
    {
        if (!_isDeleted)
            return;

        _isDeleted = false;
        foreach (var pet in _pets)
            pet.Restore();
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public int CountPetsFindHome() => _pets.Count(p => p.Status == PetStatus.FindHome);

    public int CountPetsLookingHome() => _pets.Count(p => p.Status == PetStatus.LookingHome);

    public int CountPetsInTreatment() => _pets.Count(p => p.Status == PetStatus.InTreatment);
}