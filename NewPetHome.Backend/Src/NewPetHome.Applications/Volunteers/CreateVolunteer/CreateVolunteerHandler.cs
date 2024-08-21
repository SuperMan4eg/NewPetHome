using CSharpFunctionalExtensions;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Volunteers;

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

        if(fullName.IsFailure)
            return fullName.Error;
        
        var requisites = request.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value);

        var socialNetworks = request.SocialNetwork.Select(r =>
            SocialNetwork.Create(r.Name, r.Url).Value);

        var details = new VolunteerDetails(requisites, socialNetworks);

        var volunteerResult = Volunteer.Create(
            volunteerId,
            fullName.Value,
            request.Experience,
            request.Description,
            request.PhoneNumber,
            details);
        
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        return volunteerResult.Value.Id;
    }
}