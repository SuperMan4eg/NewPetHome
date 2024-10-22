using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.DeletePetFiles;

public class DeletePetFilesCommandValidator : AbstractValidator<DeletePetFilesCommand>
{
    public DeletePetFilesCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.FilePaths).MustBeValueObject(FilePath.Create);
    }
}