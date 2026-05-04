using System.Globalization;

namespace IndividualsDirectory.Api.Middleware;

public class LocalizationMiddleware(RequestDelegate next)
{
    private const string DefaultLanguage = "en";
    private static readonly string[] SupportedLanguages = ["en", "ka"];

    public async Task InvokeAsync(HttpContext context)
    {
        var header = context.Request.Headers.AcceptLanguage.ToString();
        var language = PickLanguage(header);

        var culture = new CultureInfo(language);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await next(context);
    }

    // Pick the first supported language from an Accept-Language header.
    // Examples:
    //   "ka"            -> "ka"
    //   "ka-GE"         -> "ka"   (region fallback)
    //   "en-US;q=0.9"   -> "en"
    //   "de,fr,ka"      -> "ka"   (first match wins)
    //   ""  /  unknown  -> "en"   (default)
    private static string PickLanguage(string acceptLanguageHeader)
    {
        if (string.IsNullOrWhiteSpace(acceptLanguageHeader))
            return DefaultLanguage;

        foreach (var rawTag in acceptLanguageHeader.Split(','))
        {
            var tag = StripQualityValue(rawTag).Trim();
            if (tag.Length == 0) continue;

            // Full tag match: "ka", "en"
            if (IsSupported(tag))
                return tag.ToLowerInvariant();

            // Region fallback: "ka-GE" -> "ka", "en-US" -> "en"
            var primary = GetPrimaryLanguage(tag);
            if (primary is not null && IsSupported(primary))
                return primary.ToLowerInvariant();
        }

        return DefaultLanguage;
    }

    // "ka-GE;q=0.9" -> "ka-GE"
    private static string StripQualityValue(string tag)
    {
        var semicolonIndex = tag.IndexOf(';');
        return semicolonIndex < 0 ? tag : tag[..semicolonIndex];
    }

    // "ka-GE" -> "ka",   "ka" -> null
    private static string? GetPrimaryLanguage(string languageTag)
    {
        var dashIndex = languageTag.IndexOf('-');
        return dashIndex > 0 ? languageTag[..dashIndex] : null;
    }

    private static bool IsSupported(string language) =>
        SupportedLanguages.Contains(language, StringComparer.OrdinalIgnoreCase);
}
