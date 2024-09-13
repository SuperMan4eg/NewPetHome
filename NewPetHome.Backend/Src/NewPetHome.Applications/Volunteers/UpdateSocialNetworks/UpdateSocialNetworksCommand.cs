using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(Guid VolunteerId, IEnumerable<SocialNetworkDto> SocialNetworks);