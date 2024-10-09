namespace NewPetHome.Core.Dtos;

public class SpecieDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public BreedDto[] Breeds { get; init; } = [];
}