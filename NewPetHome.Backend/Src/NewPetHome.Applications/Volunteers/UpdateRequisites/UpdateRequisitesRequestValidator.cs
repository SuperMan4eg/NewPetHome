using FluentValidation;
using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.Validation;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Applications.Volunteers.UpdateRequisites;

public class UpdateRequisitesRequestValidator : AbstractValidator<UpdateRequisitesRequest>
{
    public UpdateRequisitesRequestValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateRequisitesDtoValidator : AbstractValidator<UpdateRequisitesDto>
{
    public UpdateRequisitesDtoValidator()
    {
        RuleForEach(u => u.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
    }
}