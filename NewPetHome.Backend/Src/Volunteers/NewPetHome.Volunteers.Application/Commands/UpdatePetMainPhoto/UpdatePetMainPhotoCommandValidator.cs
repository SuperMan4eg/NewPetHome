using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetMainPhoto;

public class UpdatePetMainPhotoCommandValidator : AbstractValidator<UpdatePetMainPhotoCommand>
{
    public UpdatePetMainPhotoCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.FilePath).MustBeValueObject(FilePath.Create);
    }
}