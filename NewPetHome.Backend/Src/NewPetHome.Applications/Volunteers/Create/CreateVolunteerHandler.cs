using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Entitys;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<VolunteerId, Error>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(request.FullName.FirstName, request.FullName.LastName).Value;

        var description = Description.Create(request.Description).Value;

        var email = Email.Create(request.Email).Value;

        var experience = Experience.Create(request.Experience).Value;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        var requisites = new RequisitesList(request.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));

        var socialNetworks = new SocialNetworks(request.SocialNetworks.Select(r =>
            SocialNetwork.Create(r.Name, r.Url).Value));

        var volunteerResult = new Volunteer(
            volunteerId,
            fullName,
            description,
            email,
            experience,
            phoneNumber,
            requisites,
            socialNetworks);

        await _volunteersRepository.Add(volunteerResult, cancellationToken);

        _logger.LogInformation("Volunteer created with id: {VolunteerId}.", volunteerId);

        return volunteerResult.Id;
    }
}