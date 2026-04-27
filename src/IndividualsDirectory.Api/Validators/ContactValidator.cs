using FluentValidation;
using IndividualsDirectory.Api.Localization;
using IndividualsDirectory.Service.Models;
using IndividualsDirectory.Service.Models.Shared;
using Microsoft.Extensions.Localization;

namespace IndividualsDirectory.Api.Validators;

public class ContactValidator : AbstractValidator<Contact>
{
    public ContactValidator(IStringLocalizer<Messages> localizer)
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage(_ => localizer["Validation_Enum_Invalid"].Value);

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage(_ => localizer["Validation_Required"].Value)
            .Length(4, 50).WithMessage(_ => localizer["Validation_Contact_Number_Length"].Value);
    }
}
