using CSharpFunctionalExtensions;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record TypeDetails
{
    private TypeDetails(SpeciesId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    
    public SpeciesId SpeciesId { get; }
    public BreedId BreedId { get; }
    
    public static Result<TypeDetails> Create(SpeciesId speciesId, BreedId breedId)
    {
        return new TypeDetails(speciesId, breedId);
    }
}