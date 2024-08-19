using NewPetHome.Domain.Volunteers;

namespace NewPetHome.Applications.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<VolunteerId> Handle(CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();

        var requisites = request.Requisites.Select(r => Requisite.Create(r.Name, r.Description)).ToList();
        var socialNetwork = request.SocialNetwork.Select(r => SocialNetwork.Create(r.Name, r.Url)).ToList();
        
        var volunteerResult = Volunteer.Create(
            volunteerId, request.FullName, request.Experience,
            request.Description, request.PhoneNumber,
            requisites, socialNetwork);
        
        await _volunteersRepository.Add(volunteerResult, cancellationToken);

        return volunteerResult.Id;
    }
}