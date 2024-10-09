using FluentValidation;
using NewPetHome.Core.Validation;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;

namespace NewPetHome.Volunteers.Application.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(r => SocialNetwork.Create(r.Name, r.Url));
    }
}