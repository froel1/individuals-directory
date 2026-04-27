using FluentValidation;
using IndividualsDirectory.Api.Localization;
using IndividualsDirectory.Api.Models;
using IndividualsDirectory.Service.Images;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace IndividualsDirectory.Api.Validators;

public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
    public UploadImageRequestValidator(
        IStringLocalizer<Messages> localizer,
        IOptions<ImageStorageOptions> options)
    {
        var opts = options.Value;

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage(_ => localizer["Validation_Image_Required"].Value)
            .Must(f => f.Length > 0)
            .WithMessage(_ => localizer["Validation_Image_Required"].Value)
            .Must(f => f.Length <= opts.MaxBytes)
            .WithMessage(_ => localizer["Validation_Image_TooLarge", opts.MaxBytes].Value)
            .Must(f => opts.AllowedContentTypes.Contains(f.ContentType, StringComparer.OrdinalIgnoreCase))
            .WithMessage(_ => localizer["Validation_Image_InvalidType", string.Join(", ", opts.AllowedContentTypes)].Value);
    }
}
