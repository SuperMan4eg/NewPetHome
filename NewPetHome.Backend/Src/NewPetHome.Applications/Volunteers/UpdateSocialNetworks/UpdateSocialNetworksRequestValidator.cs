using FluentValidation;
using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.Volunteers.UpdateSocialNetworks;

public class UpdateSocialNetworksRequestValidator: AbstractValidator<UpdateSocialNetworksRequest>
{
    public UpdateSocialNetworksRequestValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateSocialNetworksDtoValidator : AbstractValidator<UpdateSocialNetworksDto>
{
    public UpdateSocialNetworksDtoValidator()
    {
        RuleForEach(u => u.SocialNetworks)
            .MustBeValueObject(r => SocialNetwork.Create(r.Name, r.Url));
    }
}