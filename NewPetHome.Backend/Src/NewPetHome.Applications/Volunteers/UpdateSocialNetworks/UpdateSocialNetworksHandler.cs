using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.Volunteers.UpdateSocialNetworks;

public class UpdateSocialNetworksHandler
{
    private readonly ILogger<UpdateSocialNetworksHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateSocialNetworksHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateSocialNetworksRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var socialNetworks = new SocialNetworks(request.Dto.SocialNetworks.Select(r =>
            SocialNetwork.Create(r.Name, r.Url).Value));

        volunteerResult.Value.UpdateSocialNetworks(socialNetworks);

        _logger.LogInformation("Volunteer social networks update with id: {VolunteerId}.", request.VolunteerId);

        return await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
    }
}