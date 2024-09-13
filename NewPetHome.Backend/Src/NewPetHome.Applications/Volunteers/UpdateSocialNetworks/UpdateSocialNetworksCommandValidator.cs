using FluentValidation;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.Volunteers.UpdateSocialNetworks;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(r => SocialNetwork.Create(r.Name, r.Url));
    }
}