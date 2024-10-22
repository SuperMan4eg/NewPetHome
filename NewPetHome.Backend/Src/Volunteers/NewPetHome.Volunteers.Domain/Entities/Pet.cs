﻿using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Volunteers.Domain.Enums;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Domain.Entities;

public class Pet : Entity<PetId>, ISoftDeletable
{
    private bool _isDeleted = false;
    private List<Photo> _photos = [];
    private List<Requisite> _requisites = [];

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
        IEnumerable<Requisite> requisites,
        IEnumerable<Photo>? photos = null) : base(id)
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
        _requisites = requisites.ToList();
        _photos = photos?.ToList() ?? [];
    }

    public VolunteerId VolunteerId { get; private set; } = null!;
    public Name Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public Position Position { get; private set; } = default!;
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
    public IReadOnlyList<Photo> Photos => _photos;
    public IReadOnlyList<Requisite> Requisites => _requisites;

    internal void UpdateInfo(
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
        IEnumerable<Requisite> requisites)
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
        _requisites = requisites.ToList();
    }

    internal void DeletePhotos(IEnumerable<Photo> photos)
    {
        _photos.RemoveAll(photos.Contains);
    }

    internal void AddPhotos(IEnumerable<Photo> photos)
    {
        _photos.AddRange(photos.ToList());
    }

    internal UnitResult<Error> UpdateMainPhoto(Photo updatedPhoto)
    {
        var photoExists = _photos.FirstOrDefault(p => p.Path == updatedPhoto.Path);
        if (photoExists is null)
            return Errors.General.NotFound();

        _photos = _photos
            .Select(p => Photo.Create(p.Path, p.Path == updatedPhoto.Path).Value)
            .OrderByDescending(p => p.IsMain)
            .ToList();

        return Result.Success<Error>();
    }

    internal void UpdateRequisites(IEnumerable<Requisite> requisites) =>
        _requisites = requisites.ToList();

    internal void UpdateStatus(PetStatus status) =>
        Status = status;

    internal void SetPosition(Position position) =>
        Position = position;

    internal UnitResult<Error> MoveForward()
    {
        var newPosition = Position.Forward();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

    internal UnitResult<Error> MoveBack()
    {
        var newPosition = Position.Back();
        if (newPosition.IsFailure)
            return newPosition.Error;

        Position = newPosition.Value;

        return Result.Success<Error>();
    }

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
}