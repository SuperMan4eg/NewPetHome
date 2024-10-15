using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;

namespace NewPetHome.Species.Applications.Commands.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(s => s.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}