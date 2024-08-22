using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Entitys;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<VolunteerId, Error>> Handle(CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var fullName = FullName.Create(request.FirstName, request.LastName);
        if (fullName.IsFailure)
            return fullName.Error;

        var description = Description.Create(request.Description);
        if (description.IsFailure)
            return description.Error;

        var email = Email.Create(request.Email);
        if (email.IsFailure)
            return email.Error;

        var experience = Experience.Create(request.Experience);
        if (experience.IsFailure)
            return experience.Error;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        if (phoneNumber.IsFailure)
            return phoneNumber.Error;

        var requisites = new RequisitesList(request.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));

        var socialNetworks = new SocialNetworks(request.SocialNetwork.Select(r =>
            SocialNetwork.Create(r.Name, r.Url).Value));

        var volunteerResult = Volunteer.Create(
            volunteerId,
            fullName.Value,
            description.Value,
            email.Value,
            experience.Value,
            phoneNumber.Value,
            requisites,
            socialNetworks);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        return volunteerResult.Value.Id;
    }
}