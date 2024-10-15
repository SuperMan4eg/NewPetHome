namespace NewPetHome.Core.Dtos;

public record BreedDto
{
    public Guid Id { get; init; }

    public Guid SpecieId { get; init; }

    public string Name { get; init; } = string.Empty;
}