using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.Species.Domain;
using NewPetHome.Volunteers.Contracts;

namespace NewPetHome.Species.Applications.Commands.DeleteBreed;

public class DeleteBreedHandler(
    ISpeciesUnitOfWork unitOfWork,
    IVolunteersContract volunteersContract,
    ISpeciesRepository speciesRepository,
    ILogger<DeleteBreedHandler> logger,
    IValidator<DeleteBreedCommand> validator) : ICommandHandler<Guid, DeleteBreedCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var breedUsed = await volunteersContract.IsPetsUsedBreed(command.BreedId, cancellationToken);
        if (breedUsed.IsSuccess)
            return Errors.General.IsUsed(nameof(Breed), command.BreedId).ToErrorList();

        var specie = await speciesRepository.GetSpecieById(command.SpecieId, cancellationToken);
        if (specie.IsFailure)
            return specie.Error.ToErrorList();

        var breed = specie.Value.Breeds.FirstOrDefault(b => b.Id == command.BreedId);
        if (breed is null)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        specie.Value.DeleteBreed(breed);

        speciesRepository.Save(specie.Value, cancellationToken);
        
        await unitOfWork.SaveChanges(cancellationToken);

        logger.LogInformation("Breed deleted with id: {breedId}.", breed.Id.Value);

        return breed.Id.Value;
    }
}