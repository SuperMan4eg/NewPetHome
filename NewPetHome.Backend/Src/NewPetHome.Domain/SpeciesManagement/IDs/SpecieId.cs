namespace NewPetHome.Domain.SpeciesManagement.IDs;

public record SpecieId
{
    private SpecieId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static SpecieId NewSpeciesId() => new(Guid.NewGuid());

    public static SpecieId Empty() => new(Guid.Empty);

    public static SpecieId Create(Guid id) => new(id);

    public static implicit operator SpecieId(Guid id) => new(id);

    public static implicit operator Guid(SpecieId specieId)
    {
        ArgumentNullException.ThrowIfNull(specieId);

        return specieId.Value;
    }
}