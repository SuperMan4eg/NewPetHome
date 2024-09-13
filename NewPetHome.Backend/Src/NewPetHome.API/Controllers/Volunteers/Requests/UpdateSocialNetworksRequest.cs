using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.Volunteers.UpdateSocialNetworks;

namespace NewPetHome.API.Controllers.Volunteers.Requests;

public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, SocialNetworks);
}