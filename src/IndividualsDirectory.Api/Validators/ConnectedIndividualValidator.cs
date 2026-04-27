using FluentValidation;
using IndividualsDirectory.Api.Localization;
using IndividualsDirectory.Service.Models;
using Microsoft.Extensions.Localization;

namespace IndividualsDirectory.Api.Validators;

public class ConnectedIndividualValidator : AbstractValidator<ConnectedIndividual>
{
    public ConnectedIndividualValidator(IStringLocalizer<Messages> localizer)
    {
        RuleFor(x => x.IndividualId)
            .GreaterThan(0)
            .WithMessage(_ => localizer["Validation_GreaterThanZero"].Value);

        RuleFor(x => x.ConnectionType)
            .IsInEnum()
            .WithMessage(_ => localizer["Validation_Enum_Invalid"].Value);
    }
}
