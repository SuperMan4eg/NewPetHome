using NewPetHome.Core;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;

namespace NewPetHome.Species.Domain;

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