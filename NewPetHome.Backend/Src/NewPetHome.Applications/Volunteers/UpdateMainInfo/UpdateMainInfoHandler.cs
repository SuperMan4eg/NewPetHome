using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Volunteers.Create;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullName = FullName.Create(request.Dto.FullName.FirstName, request.Dto.FullName.LastName).Value;
        var description = Description.Create(request.Dto.Description).Value;
        var email = Email.Create(request.Dto.Email).Value;
        var experience = Experience.Create(request.Dto.Experience).Value;
        var phoneNumber = PhoneNumber.Create(request.Dto.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            description,
            email,
            experience,
            phoneNumber);

        _logger.LogInformation("Volunteer main info update with id: {VolunteerId}.", request.VolunteerId);
        
        return await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
    }
}