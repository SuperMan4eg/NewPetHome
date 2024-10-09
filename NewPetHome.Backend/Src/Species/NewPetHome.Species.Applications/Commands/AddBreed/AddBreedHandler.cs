﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Domain;

namespace NewPetHome.Species.Applications.Commands.AddBreed;

public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly IValidator<AddBreedCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddBreedHandler> _logger;

    public AddBreedHandler(
        IValidator<AddBreedCommand> validator,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddBreedHandler> logger)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _speciesRepository = speciesRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var specieResult = await _speciesRepository
            .GetById(SpecieId.Create(command.SpecieId), cancellationToken);
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();

        var breedId = BreedId.NewBreedId();
        var name = Name.Create(command.Name).Value;

        var breed = new Breed(breedId, name);

        var breedResult = specieResult.Value.AddBreed(breed);
        if (breedResult.IsFailure)
            return breedResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Breed added with id: {BreedId}.", breedResult.Value);

        return breed.Id.Value;
    }
}