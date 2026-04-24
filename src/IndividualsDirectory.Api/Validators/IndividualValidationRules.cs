using System.Text.RegularExpressions;
using FluentValidation;

namespace IndividualsDirectory.Api.Validators;

internal static class IndividualValidationRules
{
    private static readonly Regex GeorgianOnly = new("^[\u10D0-\u10F0]+$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
    private static readonly Regex LatinOnly = new("^[A-Za-z]+$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    public static IRuleBuilderOptions<T, string> Name<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty()
            .Length(2, 50)
            .Must(BeSingleAlphabet)
            .WithMessage("Must contain only Georgian or Latin letters, not a mix of both.");

    public static IRuleBuilderOptions<T, string> PersonalNumber<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty()
            .Matches(@"^\d{11}$")
            .WithMessage("Personal number must be exactly 11 digits.");

    public static IRuleBuilderOptions<T, DateOnly> AdultDateOfBirth<T>(this IRuleBuilder<T, DateOnly> rule) =>
        rule.Must(BeAtLeast18)
            .WithMessage("Individual must be at least 18 years old.");

    private static bool BeSingleAlphabet(string value) =>
        !string.IsNullOrEmpty(value) && (GeorgianOnly.IsMatch(value) || LatinOnly.IsMatch(value));

    private static bool BeAtLeast18(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return dob <= today.AddYears(-18);
    }
}
