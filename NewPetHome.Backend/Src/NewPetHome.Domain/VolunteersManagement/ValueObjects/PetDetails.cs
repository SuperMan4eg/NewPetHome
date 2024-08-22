using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record PetDetails
{
    private readonly List<Requisite> _requisites;
    private readonly List<PetPhoto> _photos;

    private PetDetails()
    {
    }

    private PetDetails(List<Requisite> requisites, List<PetPhoto> photos)
    {
        _requisites = requisites ??= [];
        _photos = photos ??= [];
    }

    public IReadOnlyList<Requisite> Requisites => _requisites;
    public IReadOnlyList<PetPhoto> Photos => _photos;
}