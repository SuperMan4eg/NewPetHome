using FluentValidation;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.SpeciesManagement.Commands.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}