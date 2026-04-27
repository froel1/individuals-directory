using FluentValidation;
using IndividualsDirectory.Api.Localization;
using IndividualsDirectory.Service.Models;
using Microsoft.Extensions.Localization;

namespace IndividualsDirectory.Api.Validators;

public class CreateIndividualRequestValidator : AbstractValidator<CreateIndividualRequest>
{
    public CreateIndividualRequestValidator(IStringLocalizer<Messages> localizer)
    {
        RuleFor(x => x.FirstName).Name(localizer);
        RuleFor(x => x.LastName).Name(localizer);

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage(_ => localizer["Validation_Enum_Invalid"].Value);

        RuleFor(x => x.PersonalNumber).PersonalNumber(localizer);
        RuleFor(x => x.DateOfBirth).AdultDateOfBirth(localizer);

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .WithMessage(_ => localizer["Validation_GreaterThanZero"].Value);

        RuleForEach(x => x.Contacts).SetValidator(new ContactValidator(localizer));

        When(x => x.ConnectedIndividuals != null, () =>
        {
            RuleForEach(x => x.ConnectedIndividuals!).SetValidator(new ConnectedIndividualValidator(localizer));
        });
    }
}
