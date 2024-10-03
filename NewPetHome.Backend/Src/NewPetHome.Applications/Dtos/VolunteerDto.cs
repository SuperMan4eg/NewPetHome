namespace NewPetHome.Applications.Dtos;

public class VolunteerDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public int Experience { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public PetDto[] Pets { get; init; } = [];

    public RequisiteDto[] Requisites { get; set; } = [];

    public SocialNetworkDto[] SocialNetworks { get; set; } = [];
}