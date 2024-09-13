using FluentValidation;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Applications.Volunteers.Delete;

public class DeleteVolunteerCommandValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}