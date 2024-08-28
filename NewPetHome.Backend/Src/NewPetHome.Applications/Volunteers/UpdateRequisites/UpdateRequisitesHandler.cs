using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Volunteers.Create;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.Volunteers.UpdateRequisites;

public class UpdateRequisitesHandler
{
    private readonly ILogger<UpdateRequisitesHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;

    public UpdateRequisitesHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateRequisitesHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateRequisitesRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var requisites = new RequisitesList(request.Dto.Requisites.Select(r =>
            Requisite.Create(r.Name, r.Description).Value));

        volunteerResult.Value.UpdateRequisites(requisites);

        _logger.LogInformation("Volunteer requisites update with id: {VolunteerId}.", request.VolunteerId);

        return await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);
    }
}