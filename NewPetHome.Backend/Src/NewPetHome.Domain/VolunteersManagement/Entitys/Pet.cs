using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.Entitys;

public class Pet : Entity<PetId>
{
    private Pet(PetId id) : base(id)
    {
    }
    
    public string Name { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public TypeDetails TypeDetails { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public string HealthInfo { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public PetStatus Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public PetDetails Details { get; private set; } = default!;
}