namespace NewPetHome.Domain;

public class Pet
{
    public Guid Id { get;private set; }
    public string Name { get;private set; }
    public string species { get;private set; }
    public string Description { get;private set; }
    public string Breed { get;private set; }
    public string Color { get;private set; }
    public string HealthInfo { get;private set; }
    public string Address { get;private set; }
    public int Weight { get;private set; }
    public int Height { get;private set; }
    public string PhoneNumber { get;private set; }
    public bool IsCastrated { get;private set; }
    public DateOnly BirthDate { get;private set; }
    public bool IsVaccinated { get;private set; }
    public string Status { get;private set; }
    public Requisites Requisites { get;private set; }
    public DateTime CreatedDate { get; private set; }
}