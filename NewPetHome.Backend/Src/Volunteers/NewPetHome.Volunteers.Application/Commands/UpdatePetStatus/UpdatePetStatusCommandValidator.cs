using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Status).Must(s => Constants.PERMITTED_PET_STATUSES_FROM_VOLUNTEER.Contains(s));
    }
}