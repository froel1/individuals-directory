using FluentValidation;
using IndividualsDirectory.Service.Models;

namespace IndividualsDirectory.Api.Validators;

public class CreateIndividualRequestValidator : AbstractValidator<CreateIndividualRequest>
{
    public CreateIndividualRequestValidator()
    {
        RuleFor(x => x.FirstName).Name();
        RuleFor(x => x.LastName).Name();
        RuleFor(x => x.Gender).IsInEnum();
        RuleFor(x => x.PersonalNumber).PersonalNumber();
        RuleFor(x => x.DateOfBirth).AdultDateOfBirth();
        RuleFor(x => x.CityId).GreaterThan(0);

        RuleForEach(x => x.Contacts).SetValidator(new ContactValidator());

        When(x => x.ConnectedIndividuals != null, () =>
        {
            RuleForEach(x => x.ConnectedIndividuals!).SetValidator(new ConnectedIndividualValidator());
        });
    }
}
