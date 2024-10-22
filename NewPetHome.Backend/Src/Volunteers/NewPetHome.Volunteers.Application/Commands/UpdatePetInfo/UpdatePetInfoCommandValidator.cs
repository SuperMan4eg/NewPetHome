using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.UpdatePetInfo;

public class UpdatePetInfoCommandValidator:AbstractValidator<UpdatePetInfoCommand>
{
    public UpdatePetInfoCommandValidator()
    {
        RuleFor(a => a.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(a => a.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(a => a.Name).MustBeValueObject(Name.Create);
        RuleFor(a => a.Description).MustBeValueObject(Description.Create);
        RuleFor(a => a.SpecieId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(a => a.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(a => a.Color).MustBeValueObject(Color.Create);
        RuleFor(a => a.HealthInfo).MustBeValueObject(HealthInfo.Create);

        RuleFor(a => a.Address)
            .MustBeValueObject(a => Address.Create(a.City, a.Street, a.HouseNumber));

        RuleFor(a => a.Weight).MustBeValueObject(Weight.Create);
        RuleFor(a => a.Height).MustBeValueObject(Height.Create);
        RuleFor(a => a.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleFor(a => a.IsCastrated);
        RuleFor(a => a.BirthDate).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(a => a.IsVaccinated);

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}