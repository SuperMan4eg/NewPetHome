using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.Volunteers.Domain.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.LastName));

        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Url));

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}