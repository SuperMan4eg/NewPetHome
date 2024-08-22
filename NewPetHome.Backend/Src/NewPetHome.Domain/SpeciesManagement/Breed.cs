using NewPetHome.Domain.Shared;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Domain.SpeciesManagement;

public class Breed : Entity<BreedId>
{
    private Breed(BreedId id) : base(id)
    {
    }

    public string Name { get; private set; } = default!;
}