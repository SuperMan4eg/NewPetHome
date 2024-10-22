namespace NewPetHome.Volunteers.Contracts.Requests;

public record UpdatePetStatusRequest(Guid VolunteerId, Guid PetId, string Status);