using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Volunteers.Domain.Enums;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Domain.Entities;

public class Pet : Core.Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;

    private Pet(PetId id) : base(id)
    {
    }

    public Pet(
        PetId id,
        Name name,
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
        PetStatus? status,
        ValueObjectList<Photo>? photos,
        ValueObjectList<Requisite> requisites) : base(id)
    {
        Name = name;
        Description = description;
        TypeDetails = typeDetails;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        IsCastrated = isCastrated;
        BirthDate = birthDate;
        IsVaccinated = isVaccinated;
        Status = status;
        CreatedDate = DateTime.Now.ToUniversalTime();
        Photos = photos ?? new ValueObjectList<Photo>([]);
        Requisites = requisites;
    }

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public Position Position { get; private set; }
    public TypeDetails TypeDetails { get; private set; } = default!;
    public Color Color { get; private set; } = default!;
    public HealthInfo HealthInfo { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public Weight Weight { get; private set; } = default!;
    public Height Height { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; }
    public DateTime BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public PetStatus? Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public ValueObjectList<Photo> Photos { get; private set; } = default!;
    public ValueObjectList<Requisite> Requisites { get; private set; }

    public void UpdatePhotos(ValueObjectList<Photo> photos) =>
        Photos = photos;

    public void UpdateRequisites(ValueObjectList<Requisite> requisites) =>
        Requisites = requisites;

    public void SetPosition(Position position) =>
        Position = position;

    public void SoftDelete()
    {
        if (_isDeleted)
            return;

        _isDeleted = true;
    }

    public void Restore()
    {
        if (!_isDeleted)
            return;

        _isDeleted = false;
    }

    public UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;
        
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;
        
        return Result.Success<Error>();
    }
}