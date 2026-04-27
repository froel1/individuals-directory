using FluentValidation;
using IndividualsDirectory.Api.Localization;
using IndividualsDirectory.Service.Models;
using Microsoft.Extensions.Localization;

namespace IndividualsDirectory.Api.Validators;

public class UpdateIndividualRequestValidator : AbstractValidator<UpdateIndividualRequest>
{
    public UpdateIndividualRequestValidator(IStringLocalizer<Messages> localizer)
    {
        RuleFor(x => x)
            .Must(HaveAtLeastOneField)
            .WithMessage(_ => localizer["Validation_Update_AtLeastOneField"].Value);

        When(x => x.FirstName != null, () =>
            RuleFor(x => x.FirstName!).Name(localizer));

        When(x => x.LastName != null, () =>
            RuleFor(x => x.LastName!).Name(localizer));

        When(x => x.Gender.HasValue, () =>
            RuleFor(x => x.Gender!.Value)
                .IsInEnum()
                .WithMessage(_ => localizer["Validation_Enum_Invalid"].Value));

        When(x => x.PersonalNumber != null, () =>
            RuleFor(x => x.PersonalNumber!).PersonalNumber(localizer));

        When(x => x.DateOfBirth.HasValue, () =>
            RuleFor(x => x.DateOfBirth!.Value).AdultDateOfBirth(localizer));

        When(x => x.CityId.HasValue, () =>
            RuleFor(x => x.CityId!.Value)
                .GreaterThan(0)
                .WithMessage(_ => localizer["Validation_GreaterThanZero"].Value));

        When(x => x.Contacts != null, () =>
            RuleForEach(x => x.Contacts!).SetValidator(new ContactValidator(localizer)));
    }

    private static bool HaveAtLeastOneField(UpdateIndividualRequest x) =>
        x.FirstName != null
        || x.LastName != null
        || x.Gender.HasValue
        || x.PersonalNumber != null
        || x.DateOfBirth.HasValue
        || x.CityId.HasValue
        || x.Contacts != null
        || x.ImageId.HasValue;
}