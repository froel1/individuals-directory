using FluentValidation;
using IndividualsDirectory.Service.Models;

namespace IndividualsDirectory.Api.Validators;

public class ConnectedIndividualValidator : AbstractValidator<ConnectedIndividual>
{
    public ConnectedIndividualValidator()
    {
        RuleFor(x => x.IndividualId).GreaterThan(0);
        RuleFor(x => x.ConnectionType).IsInEnum();
    }
}
