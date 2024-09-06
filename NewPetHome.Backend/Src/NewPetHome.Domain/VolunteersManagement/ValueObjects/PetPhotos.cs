using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record PetPhotos
{
    private PetPhotos()
    {
    }

    public PetPhotos(IEnumerable<Photo> photos)
    {
        Photos = photos.ToList();
    }

    public IReadOnlyList<Photo> Photos { get; }
}