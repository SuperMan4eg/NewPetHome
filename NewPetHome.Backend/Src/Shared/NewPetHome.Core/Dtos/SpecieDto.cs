namespace NewPetHome.Core.Dtos;

public record SpecieDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public IEnumerable<BreedDto> Breeds { get; init; } = default!;
}