using FluentValidation;
using IndividualsDirectory.Service.Models;

namespace IndividualsDirectory.Api.Validators;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator()
    {
        RuleFor(x => x.Type).IsInEnum();

        RuleFor(x => x.Number)
            .NotEmpty()
            .Length(4, 50);
    }
}
