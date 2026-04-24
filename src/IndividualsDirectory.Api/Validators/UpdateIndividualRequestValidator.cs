using FluentValidation;
using IndividualsDirectory.Service.Models;

namespace IndividualsDirectory.Api.Validators;

public class UpdateIndividualRequestValidator : AbstractValidator<UpdateIndividualRequest>
{
    public UpdateIndividualRequestValidator()
    {
        RuleFor(x => x)
            .Must(HaveAtLeastOneField)
            .WithMessage("At least one field must be provided.");

        When(x => x.FirstName != null, () =>
            RuleFor(x => x.FirstName!).Name());

        When(x => x.LastName != null, () =>
            RuleFor(x => x.LastName!).Name());

        When(x => x.Gender.HasValue, () =>
            RuleFor(x => x.Gender!.Value).IsInEnum());

        When(x => x.PersonalNumber != null, () =>
            RuleFor(x => x.PersonalNumber!).PersonalNumber());

        When(x => x.DateOfBirth.HasValue, () =>
            RuleFor(x => x.DateOfBirth!.Value).AdultDateOfBirth());

        When(x => x.CityId.HasValue, () =>
            RuleFor(x => x.CityId!.Value).GreaterThan(0));

        When(x => x.Contacts != null, () =>
            RuleForEach(x => x.Contacts!).SetValidator(new ContactValidator()));
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
