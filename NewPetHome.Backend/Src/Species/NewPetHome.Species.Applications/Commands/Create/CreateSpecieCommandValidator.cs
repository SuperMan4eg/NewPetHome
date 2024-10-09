using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel.ValueObjects;

namespace NewPetHome.Species.Applications.Commands.Create;

public class CreateSpecieCommandValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpecieCommandValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}