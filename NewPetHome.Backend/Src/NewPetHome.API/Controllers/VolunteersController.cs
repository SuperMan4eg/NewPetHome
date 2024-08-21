using Microsoft.AspNetCore.Mvc;
using NewPetHome.API.Extensions;
using NewPetHome.API.Response;
using NewPetHome.Applications.Volunteers.CreateVolunteer;

namespace NewPetHome.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(Envelope.Ok(result.Value.Value));
    }
}