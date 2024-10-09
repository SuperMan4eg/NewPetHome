using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Species.Domain;

namespace NewPetHome.Species.Applications.Commands.Create;

public class CreateSpecieHandler : ICommandHandler<Guid, CreateSpecieCommand>
{
    private readonly IValidator<CreateSpecieCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSpecieHandler> _logger;

    public CreateSpecieHandler(
        IValidator<CreateSpecieCommand> validator,
        ISpeciesRepository speciesRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateSpecieHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var specieId = SpecieId.NewSpeciesId();
        var name = Name.Create(command.Name).Value;

        var specie = new Specie(specieId, name);

        var specieResult = await _speciesRepository.Add(specie, cancellationToken);
        if (specieResult.IsFailure)
            return specieResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Specie created with id: {specieId}.", specieResult.Value);

        return specie.Id.Value;
    }
}