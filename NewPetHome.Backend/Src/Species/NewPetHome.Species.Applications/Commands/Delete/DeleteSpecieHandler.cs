using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.Volunteers.Contracts;

namespace NewPetHome.Species.Applications.Commands.Delete;

public class DeleteSpecieHandler(
    ISpeciesUnitOfWork unitOfWork,
    IVolunteersContract volunteersContract,
    ISpeciesRepository speciesRepository,
    ILogger<DeleteSpecieHandler> logger,
    IValidator<DeleteSpecieCommand> validator) : ICommandHandler<Guid, DeleteSpecieCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var specieUsed = await volunteersContract.IsPetsUsedSpecie(command.SpecieId, cancellationToken);
        if (specieUsed.IsSuccess)
            return Errors.General.IsUsed(nameof(Species), command.SpecieId).ToErrorList();

        var specie = await speciesRepository.GetSpecieById(command.SpecieId, cancellationToken);
        if (specie.IsFailure)
            return specie.Error.ToErrorList();

        var resultId = await speciesRepository.Delete(specie.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.LogInformation("Specie deleted with id: {specieId}.", resultId);

        return resultId;
    }
}