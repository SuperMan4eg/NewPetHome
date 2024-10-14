using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;

namespace NewPetHome.Species.Domain;

public class Specie : Entity<SpecieId>
{
    private readonly List<Breed> _breeds = [];

    private Specie(SpecieId id) : base(id)
    {
    }

    public Specie(SpecieId id, Name name) : base(id)
    {
        Name = name;
    }

    public Name Name { get; private set; } = default!;

    public IReadOnlyList<Breed> Breeds => _breeds;

    public Result<Guid, Error> AddBreed(Breed breed)
    {
        if (_breeds.FirstOrDefault(b => b.Name == breed.Name) is not null)
            return Errors.General.AlreadyExists(nameof(Breed), nameof(breed.Name).ToLower(), breed.Name.Value);

        _breeds.Add(breed);

        return breed.Id.Value;
    }

    public Result<Guid, Error> DeleteBreed(Breed breed)
    {
        _breeds.Remove(breed);

        return breed.Id.Value;
    }
}