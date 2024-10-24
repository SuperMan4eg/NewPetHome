using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewPetHome.Framework;
using NewPetHome.Volunteers.Application.Commands.AddPet;
using NewPetHome.Volunteers.Application.Commands.Create;
using NewPetHome.Volunteers.Application.Commands.Delete;
using NewPetHome.Volunteers.Application.Commands.DeletePetFiles;
using NewPetHome.Volunteers.Application.Commands.HardDeletePetById;
using NewPetHome.Volunteers.Application.Commands.SoftDeletePetById;
using NewPetHome.Volunteers.Application.Commands.UpdateMainInfo;
using NewPetHome.Volunteers.Application.Commands.UpdatePetInfo;
using NewPetHome.Volunteers.Application.Commands.UpdatePetMainPhoto;
using NewPetHome.Volunteers.Application.Commands.UpdatePetStatus;
using NewPetHome.Volunteers.Application.Commands.UpdateRequisites;
using NewPetHome.Volunteers.Application.Commands.UpdateSocialNetworks;
using NewPetHome.Volunteers.Application.Commands.UploadFilesToPet;
using NewPetHome.Volunteers.Application.Queries.GetFilteredPetsWithPagination;
using NewPetHome.Volunteers.Application.Queries.GetPetById;
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

    [HttpGet("pets")]
    public async Task<ActionResult> GetPets(
        [FromQuery] GetFilteredPetsWithPaginationRequest request,
        [FromServices] GetFilteredPetsWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetFilteredPetsWithPaginationQuery(
            request.VolunteerId,
            request.Name,
            request.AgeFrom,
            request.AgeTo,
            request.WeightFrom,
            request.WeightTo,
            request.HeightFrom,
            request.HeightTo,
            request.SpeciesId,
            request.BreedId,
            request.Color,
            request.IsCastrated,
            request.IsVaccinated,
            request.Status,
            request.City,
            request.Street,
            request.SortBy,
            request.SortDirection,
            request.Page,
            request.PageSize);

        var result = await handler.Handle(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid petId,
        [FromServices] GetPetByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPetByIdQuery(petId);

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

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/info")]
    public async Task<ActionResult> UpdatePetInfo(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] UpdatePetInfoHandler handler,
        [FromBody] UpdatePetInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdatePetInfoCommand(
            volunteerId,
            petId,
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
            request.Requisites
        );

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/status")]
    public async Task<ActionResult> UpdatePetStatus(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetStatusRequest request,
        [FromServices] UpdatePetStatusHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdatePetStatusCommand(volunteerId, petId, request.Status);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/pet/{petId:guid}/main-photo")]
    public async Task<ActionResult> UpdatePetMainPhoto(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetMainPhotoRequest request,
        [FromServices] UpdatePetMainPhotoHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdatePetMainPhotoCommand(volunteerId, petId, request.FilePath);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/files")]
    public async Task<ActionResult> DeletePetFiles(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] DeletePetFilesRequest request,
        [FromServices] DeletePetFilesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new DeletePetFilesCommand(volunteerId, petId, request.FilePaths);

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

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult> SoftDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] SoftDeletePetByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new SoftDeletePetByIdCommand(volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult> HardDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] HardDeletePetByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new HardDeletePetByIdCommand(volunteerId, petId);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}