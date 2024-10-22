using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Commands.HardDeletePetById;

public class HardDeletePetByIdCommandValidator : AbstractValidator<HardDeletePetByIdCommand>
{
    public HardDeletePetByIdCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}