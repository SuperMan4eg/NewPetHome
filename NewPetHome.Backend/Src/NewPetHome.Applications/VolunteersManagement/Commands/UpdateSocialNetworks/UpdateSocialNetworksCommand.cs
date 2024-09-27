using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Dtos;

namespace NewPetHome.Applications.VolunteersManagement.Commands.UpdateSocialNetworks;

public record UpdateSocialNetworksCommand(
    Guid VolunteerId, IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;