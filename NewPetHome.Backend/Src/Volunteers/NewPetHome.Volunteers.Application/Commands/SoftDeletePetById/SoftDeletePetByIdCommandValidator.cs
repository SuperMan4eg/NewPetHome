using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Commands.SoftDeletePetById;

public class SoftDeletePetByIdCommandValidator : AbstractValidator<SoftDeletePetByIdCommand>
{
    public SoftDeletePetByIdCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}