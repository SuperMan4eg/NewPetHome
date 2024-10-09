using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel.ValueObjects;

namespace NewPetHome.Species.Applications.Commands.AddBreed;

public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedCommandValidator()
    {
        RuleFor(c => c.Name)
            .MustBeValueObject(Name.Create);
    }
}