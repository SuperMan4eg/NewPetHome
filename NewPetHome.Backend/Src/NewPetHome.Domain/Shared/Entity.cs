namespace NewPetHome.Domain.Shared;

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}