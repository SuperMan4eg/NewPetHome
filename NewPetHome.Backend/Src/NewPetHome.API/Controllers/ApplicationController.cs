using Microsoft.AspNetCore.Mvc;
using NewPetHome.API.Response;

namespace NewPetHome.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        return base.Ok(envelope);
    }
}