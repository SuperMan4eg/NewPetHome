using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Domain.SpeciesManagement;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }

    public Breed(BreedId id, Name name) : base(id)
    {
        Name = name;
        
    }

    public Name Name { get; private set; } = default!;
}