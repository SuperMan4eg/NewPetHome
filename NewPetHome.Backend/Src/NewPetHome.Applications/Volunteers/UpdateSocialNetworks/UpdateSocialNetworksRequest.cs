using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksRequest(Guid VolunteerId, UpdateSocialNetworksDto Dto);