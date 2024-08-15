namespace NewPetHome.Domain.Volunteers;

public record PetDetails
{
    private readonly List<Requisites> _requisites;
    private readonly List<PetPhoto> _photos;
    
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public IReadOnlyList<PetPhoto> Photos => _photos;
}