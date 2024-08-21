using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Species;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    public string Name { get; private set; } = default!;
    
    public IReadOnlyList<Breed> Breeds => _breeds;
}