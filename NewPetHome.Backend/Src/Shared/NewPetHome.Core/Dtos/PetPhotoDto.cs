namespace NewPetHome.Core.Dtos;

public record PetPhotoDto
{
    public string PathToStorage { get; init; } = string.Empty;
    public bool IsMain { get; init; }
}