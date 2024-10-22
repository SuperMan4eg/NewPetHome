using Microsoft.AspNetCore.Mvc;
using NewPetHome.Framework;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Applications.Commands.AddBreed;
using NewPetHome.Species.Applications.Commands.Create;
using NewPetHome.Species.Applications.Commands.Delete;
using NewPetHome.Species.Applications.Commands.DeleteBreed;
using NewPetHome.Species.Applications.Queries.GetAllSpeciesWithPagination;
using NewPetHome.Species.Applications.Queries.GetBreedsBySpecieId;
using NewPetHome.Species.Contracts.Requests;

namespace NewPetHome.Species.Presentation;

public class SpeciesController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetAllSpeciesWithPaginationRequest request,
        [FromServices] GetAllSpecieWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllSpeciesWithPaginationQuery(request.SortDirection, request.Page, request.PageSize);

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("breeds")]
    public async Task<ActionResult> GetBreedBySpecieId(
        [FromQuery] GetBreedsBySpecieIdRequest request,
        [FromServices] GetBreedsBySpecieIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetBreedsBySpecieIdQuery(request.SpecieId, request.Page, request.PageSize);

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSpecieHandler handler,
        [FromBody] CreateSpecieRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateSpecieCommand(request.Name);

        var result = await handler.Handle(command, cancellationToken);
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
        var command = new AddBreedCommand(id, request.Name);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{specieId:guid}/breed")]
    public async Task<ActionResult<Guid>> DeleteBreed(
        [FromRoute] Guid specieId,
        [FromBody] DeleteBreedRequest request,
        [FromServices] DeleteBreedHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteBreedCommand(specieId, request.BreedId);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete]
    public async Task<ActionResult<Guid>> Delete(
        [FromBody] DeleteSpecieRequest request,
        [FromServices] DeleteSpecieHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteSpecieCommand(request.SpecieId);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}