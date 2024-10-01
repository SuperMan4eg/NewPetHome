using Microsoft.AspNetCore.Mvc;
using NewPetHome.API.Controllers.Species.Requests;
using NewPetHome.API.Extensions;
using NewPetHome.Applications.SpeciesManagement.Commands.AddBreed;
using NewPetHome.Applications.SpeciesManagement.Commands.Create;

namespace NewPetHome.API.Controllers.Species;

public class SpeciesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpecieHandler handler,
        [FromBody] CreateSpecieRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("{id:guid}/breed")]
    public async Task<ActionResult<Guid>> AddBreed(
        [FromRoute] Guid id,
        [FromServices] AddBreedHandler handler,
        [FromBody] AddBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}