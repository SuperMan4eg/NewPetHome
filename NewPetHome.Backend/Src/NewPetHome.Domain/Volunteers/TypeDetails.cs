namespace NewPetHome.Domain.Volunteers;

public record TypeDetails
{
    private TypeDetails(SpeciesId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
    }
    
    public SpeciesId SpeciesId { get; }
    public BreedId BreedId { get; }
    
    public static TypeDetails Create(SpeciesId speciesId, BreedId breedId)
    {
        if(speciesId is null)
            throw new ArgumentNullException(nameof(speciesId));
        
        if(breedId is null)
            throw new ArgumentNullException(nameof(breedId));
        
        return new TypeDetails(speciesId, breedId);
    }
}