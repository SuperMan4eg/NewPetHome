using CSharpFunctionalExtensions;
using NewPetHome.SharedKernel.ValueObjects.Ids;

namespace NewPetHome.SharedKernel.ValueObjects;

public record TypeDetails
{
    private TypeDetails(SpecieId specieId, BreedId breedId)
    {
        SpecieId = specieId;
        BreedId = breedId;
    }
    
    public SpecieId SpecieId { get; }
    public BreedId BreedId { get; }
    
    public static Result<TypeDetails> Create(SpecieId specieId, BreedId breedId)
    {
        return new TypeDetails(specieId, breedId);
    }
}