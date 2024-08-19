using Microsoft.AspNetCore.Mvc;
using NewPetHome.Applications.Volunteers.CreateVolunteer;

namespace NewPetHome.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request, cancellationToken);

        return Ok(result.Value);
    }
}