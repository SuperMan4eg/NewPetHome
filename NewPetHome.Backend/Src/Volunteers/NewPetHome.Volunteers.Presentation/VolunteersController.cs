using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewPetHome.Framework;
using NewPetHome.Volunteers.Application.Commands.AddPet;
using NewPetHome.Volunteers.Application.Commands.Create;
using NewPetHome.Volunteers.Application.Commands.Delete;
using NewPetHome.Volunteers.Application.Commands.UpdateMainInfo;
using NewPetHome.Volunteers.Application.Commands.UpdateRequisites;
using NewPetHome.Volunteers.Application.Commands.UpdateSocialNetworks;
using NewPetHome.Volunteers.Application.Commands.UploadFilesToPet;
using NewPetHome.Volunteers.Application.Queries.GetVolunteerById;
using NewPetHome.Volunteers.Application.Queries.GetVolunteersWithPagination;
using NewPetHome.Volunteers.Contracts.Converters;
using NewPetHome.Volunteers.Contracts.Requests;

namespace NewPetHome.Volunteers.Presentation;

public class VolunteersController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetFilteredVolunteersWithPaginationRequest request,
        [FromServices] GetFilteredVolunteersWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFilteredVolunteersWithPaginationQuery(
            request.FirstName,
            request.ExperienceFrom,
            request.ExperienceTo,
            request.Page,
            request.PageSize);

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("dapper")]
    public async Task<ActionResult> GetById(
        [FromQuery] GetVolunteerByIdRequest request,
        [FromServices] GetVolunteerByIdHandlerDapper handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetVolunteerByIdQuery(request.Id);

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateVolunteerCommand(
            request.FullName,
            request.Description,
            request.Email,
            request.Experience,
            request.PhoneNumber,
            request.SocialNetworks,
            request.Requisites);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromServices] UpdateMainInfoHandler handler,
        [FromBody] UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateMainInfoCommand(
            id,
            request.FullName,
            request.Description,
            request.Email,
            request.Experience,
            request.PhoneNumber);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid id,
        [FromServices] UpdateRequisitesHandler handler,
        [FromBody] UpdateRequisitesRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateRequisitesCommand(id, request.Requisites);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] Guid id,
        [FromServices] UpdateSocialNetworksHandler handler,
        [FromBody] UpdateSocialNetworksRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateSocialNetworksCommand(id, request.SocialNetworks);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteVolunteerCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromBody] AddPetRequest request,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new AddPetCommand(
            id,
            request.Name,
            request.Description,
            request.SpecieId,
            request.BreedId,
            request.Color,
            request.HealthInfo,
            request.Address,
            request.Weight,
            request.Height,
            request.PhoneNumber,
            request.IsCastrated,
            request.BirthDate,
            request.IsVaccinated,
            request.Status,
            request.Requisites);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/files")]
    public async Task<ActionResult> UploadFilesToPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] UploadFilesToPetHandler handler,
        [FromServices] IFormFileConverter fileConverter,
        CancellationToken cancellationToken = default)
    {
        var fileDtos = fileConverter.ToUploadFileDtos(files);

        var command = new UploadFilesToPetCommand(volunteerId, petId, fileDtos);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}