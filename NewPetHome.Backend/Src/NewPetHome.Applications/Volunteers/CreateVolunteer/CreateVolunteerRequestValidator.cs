using FluentValidation;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Applications.Volunteers.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => new { c.FirstName, c.LastName })
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