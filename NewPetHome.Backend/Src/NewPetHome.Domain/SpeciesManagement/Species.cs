using NewPetHome.Domain.Shared;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Domain.SpeciesManagement;

public class Species : Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = [];
    
    private Species(SpeciesId id) : base(id)
    {
    }
    
    public string Name { get; private set; } = default!;
    
    public IReadOnlyList<Breed> Breeds => _breeds;
}