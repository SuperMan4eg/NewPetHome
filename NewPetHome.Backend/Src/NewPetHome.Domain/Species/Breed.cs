using NewPetHome.Domain.Shared;

namespace NewPetHome.Domain.Species;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }

    public string Name { get; private set; } = default!;
}