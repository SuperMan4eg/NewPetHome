using FluentValidation;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.SpeciesManagement.Commands.Create;

public class CreateSpecieCommandValidator : AbstractValidator<CreateSpecieCommand>
{
    public CreateSpecieCommandValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}