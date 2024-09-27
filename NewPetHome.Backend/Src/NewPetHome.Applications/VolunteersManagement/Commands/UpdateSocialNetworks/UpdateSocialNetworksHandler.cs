using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Extensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.VolunteersManagement.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
{
    private readonly ILogger<UpdateSocialNetworksHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateSocialNetworksCommand> _validator;

    public UpdateSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateSocialNetworksCommand> validator,
        ILogger<UpdateSocialNetworksHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var socialNetworks = new ValueObjectList<SocialNetwork>(command.SocialNetworks.Select(r =>
            SocialNetwork.Create(r.Name, r.Url).Value));

        volunteerResult.Value.UpdateSocialNetworks(socialNetworks);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer social networks update with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}