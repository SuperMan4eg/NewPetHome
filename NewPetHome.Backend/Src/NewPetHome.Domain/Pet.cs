using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain;

public class Pet : Entity<PetId>
{
    private Pet(PetId id) : base(id)
    {
    }
    
    public string Name { get; private set; } = default!;
    public string Species { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Breed { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public string HealthInfo { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public string Status { get; private set; } = default!;
    public DateTime CreatedDate { get; private set; }
    public PetDetails Details { get; private set; } = default!;
}