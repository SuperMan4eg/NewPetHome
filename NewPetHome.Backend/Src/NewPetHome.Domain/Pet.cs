namespace NewPetHome.Domain;

public class Pet
{
    private readonly List<Requisites> _requisites;
    private readonly List<PetPhoto> _photos;
    
    public Guid Id { get;private set; }
    public string Name { get;private set; }
    public string Species { get;private set; }
    public string Description { get;private set; }
    public string Breed { get;private set; }
    public string Color { get;private set; }
    public string HealthInfo { get;private set; }
    public string Address { get;private set; }
    public double Weight { get;private set; }
    public double Height { get;private set; }
    public string PhoneNumber { get;private set; }
    public bool IsCastrated { get;private set; }
    public DateOnly BirthDate { get;private set; }
    public bool IsVaccinated { get;private set; }
    public string Status { get;private set; }
    public IReadOnlyList<Requisites> Requisites => _requisites;
    public DateTime CreatedDate { get; private set; }
    public IReadOnlyList<PetPhoto> Photos => _photos;
}