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

        var fullName = FullName.Create(request.FirstName, request.LastName).Value;

        var description = Description.Create(request.Description).Value;

        var email = Email.Create(request.Email).Value;

        var experience = Experience.Create(request.Experience).Value;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        var requisites = new RequisitesList(request.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));

        var socialNetworks = new SocialNetworks(request.SocialNetworks.Select(r =>
            SocialNetwork.Create(r.Name, r.Url).Value));

        var volunteerResult = Volunteer.Create(
            volunteerId,
            fullName,
            description,
            email,
            experience,
            phoneNumber,
            requisites,
            socialNetworks);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        return volunteerResult.Value.Id;
    }
}