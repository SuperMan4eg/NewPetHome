using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Volunteers.Domain.Enums;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Domain.Entities;

public sealed class Volunteer : Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private readonly List<Pet> _pets = [];
    private List<Requisite> _requisites = [];
    private List<SocialNetwork> _socialNetworks = [];

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
        List<Requisite> requisites,
        List<SocialNetwork> socialNetworks) :
        base(id)
    {
        FullName = fullName;
        Description = description;
        Email = email;
        Experience = experience;
        PhoneNumber = phoneNumber;
        _requisites = requisites;
        _socialNetworks = socialNetworks;
    }

    public FullName FullName { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Experience Experience { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public IReadOnlyList<Pet> Pets => _pets;
    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet is null)
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

    public void UpdateRequisites(List<Requisite> requisites)
    {
        _requisites = requisites;
    }

    public void UpdateSocialNetworks(List<SocialNetwork> socialNetworks)
    {
        _socialNetworks = socialNetworks;
    }

    public void SoftDelete()
    {
        if (_isDeleted)
            return;

        _isDeleted = true;
        foreach (var pet in _pets)
            pet.SoftDelete();
    }

    public void Restore()
    {
        if (!_isDeleted)
            return;

        _isDeleted = false;
        foreach (var pet in _pets)
            pet.Restore();
    }

    public void SoftDeletePet(PetId id)
    {
        _pets.FirstOrDefault(p => p.Id == id)?.SoftDelete();
    }

    public void RestorePet(PetId id)
    {
        _pets.FirstOrDefault(p => p.Id == id)?.Restore();
    }

    public void HardDeletePet(Pet pet)
    {
        _pets.Remove(pet);
    }

    public UnitResult<Error> UpdatePetMainPhoto(Pet pet, Photo photo)
    {
        var petResult = _pets.FirstOrDefault(p => p.Id == pet.Id);
        if (petResult is null)
            return Errors.General.NotFound(pet.Id);

        var updateResult = petResult.UpdateMainPhoto(photo);
        if(updateResult.IsFailure)
            return updateResult.Error;

        return Result.Success<Error>();
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = Position.Create(_pets.Count + 1);
        if (positionResult.IsFailure)
            return positionResult.Error;

        pet.SetPosition(positionResult.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;
        if (currentPosition == newPosition || _pets.Count == 1)
            return Result.Success<Error>();

        var adjustedPosition = AdjustPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var result = MovePetsBetweenPositions(newPosition, currentPosition);
        if (result.IsFailure)
            return result.Error;

        pet.SetPosition(newPosition);

        return Result.Success<Error>();
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position newPosition, Position currentPosition)
    {
        if (newPosition < currentPosition)
        {
            var petsToMove = _pets.Where(p =>
                p.Position >= newPosition
                && p.Position < currentPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();
                if (result.IsFailure)
                    return result.Error;
            }
        }
        else if (newPosition > currentPosition)
        {
            var petsToMove = _pets.Where(p =>
                p.Position <= newPosition
                && p.Position > currentPosition);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();
                if (result.IsFailure)
                    return result.Error;
            }
        }

        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _pets.Count)
            return newPosition;

        var lastPosition = Position.Create(_pets.Count);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }

    public int CountPetsFindHome() => _pets.Count(p => p.Status == PetStatus.FindHome);

    public int CountPetsLookingHome() => _pets.Count(p => p.Status == PetStatus.LookingHome);

    public int CountPetsInTreatment() => _pets.Count(p => p.Status == PetStatus.InTreatment);

    public void AddPetPhotos(PetId petId, IEnumerable<Photo> petPhotos)
    {
        _pets.FirstOrDefault(p => p.Id == petId)?.AddPhotos(petPhotos);
    }

    public void DeletePetPhotos(PetId petId, IEnumerable<Photo> petPhotos)
    {
        _pets.FirstOrDefault(p => p.Id == petId)?.DeletePhotos(petPhotos);
    }

    public UnitResult<Error> IsPetExist(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet is null)
            return Errors.General.NotFound(petId.Value);

        return Result.Success<Error>();
    }

    public void UpdatePetInfo(PetId petId, Name name,
        Description description,
        TypeDetails typeDetails,
        Color color,
        HealthInfo healthInfo,
        Address address,
        Weight weight,
        Height height,
        PhoneNumber phoneNumber,
        bool isCastrated,
        DateTime birthDate,
        bool isVaccinated,
        IEnumerable<Requisite> requisites)
    {
        _pets.FirstOrDefault(p => p.Id == petId)?.UpdateInfo(
            name,
            description,
            typeDetails,
            color,
            healthInfo,
            address,
            weight,
            height,
            phoneNumber,
            isCastrated,
            birthDate,
            isVaccinated,
            requisites);
    }

    public void UpdatePetStatus(PetId petId, PetStatus status)
    {
        _pets.FirstOrDefault(p => p.Id == petId)?.UpdateStatus(status);
    }
}