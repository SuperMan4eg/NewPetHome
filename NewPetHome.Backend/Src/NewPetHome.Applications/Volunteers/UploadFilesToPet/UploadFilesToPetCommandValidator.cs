using FluentValidation;
using NewPetHome.Applications.Dtos.Validators;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Applications.Volunteers.UploadFilesToPet;

public class UploadFilesToPetCommandValidator : AbstractValidator<UploadFilesToPetCommand>
{
    public UploadFilesToPetCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.Files).SetValidator(new UploadFileDtoValidator());
    }
}