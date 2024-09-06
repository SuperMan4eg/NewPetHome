using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.Entities;

public class Pet : Entity<PetId>, ISoftDeletable
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
        DateOnly birthDate,
        bool isVaccinated,
        PetStatus status,
        PetPhotos photos,
        RequisitesList requisites) : base(id)
    {
        Name =  name;
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
        CreatedDate = DateTime.Now;
        Photos = photos;
        Requisites = requisites;
    }

    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public TypeDetails TypeDetails { get; private set; } = default!;
    public Color Color { get; private set; } = default!;
    public HealthInfo HealthInfo { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public Weight Weight { get; private set; } = default!;
    public Height Height { get; private set; }= default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public PetStatus Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public PetPhotos Photos { get; private set; } = default!;
    public RequisitesList Requisites { get; private set; } = default!;

    public void Delete()
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
}