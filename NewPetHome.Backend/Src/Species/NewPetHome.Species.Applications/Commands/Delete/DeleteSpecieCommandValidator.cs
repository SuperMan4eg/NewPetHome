using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;

namespace NewPetHome.Species.Applications.Commands.Delete;

public class DeleteSpecieCommandValidator : AbstractValidator<DeleteSpecieCommand>
{
    public DeleteSpecieCommandValidator()
    {
        RuleFor(s => s.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}