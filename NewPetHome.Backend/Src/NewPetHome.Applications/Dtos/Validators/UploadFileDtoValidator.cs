using FluentValidation;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Applications.Dtos.Validators;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Content).Must(c => c.Length < 5000000);
    }
}