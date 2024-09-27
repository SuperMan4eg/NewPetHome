namespace NewPetHome.Applications.Dtos;

public class PetDto
{
    public Guid Id { get; init; }

    public Guid VolunteerId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Position { get; init; }

    public Guid SpeciesId { get; init; }

    public Guid BreedId { get; init; }

    public string Color { get; init; } = string.Empty;

    public string HealthInfo { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty!;

    public string Street { get; init; } = string.Empty!;

    public int HouseNumber { get; init; }

    public double Weight { get; init; }

    public double Height { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public bool IsCastrated { get; init; }

    public DateTime BirthDate { get; init; }

    public bool IsVaccinated { get; init; }

    public string Status { get; init; } = string.Empty;

    public DateTime CreatedDate { get; init; }

    public string Photos { get; init; } = string.Empty;

    public RequisiteDto[] Requisites { get; init; } = [];
}