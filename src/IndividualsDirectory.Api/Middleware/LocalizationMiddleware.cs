using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace IndividualsDirectory.Api.Middleware;

public class LocalizationMiddleware(RequestDelegate next)
{
    private static readonly string[] SupportedCultures = ["en", "ka"];
    private const string DefaultCulture = "en";

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = ResolveCulture(context.Request.Headers.AcceptLanguage);
        var cultureInfo = new CultureInfo(culture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await next(context);
    }

    private static string ResolveCulture(IEnumerable<string?> acceptLanguageValues)
    {
        var headerValue = string.Join(",", acceptLanguageValues.Where(v => !string.IsNullOrWhiteSpace(v)));
        if (string.IsNullOrWhiteSpace(headerValue))
        {
            return DefaultCulture;
        }

        if (!StringWithQualityHeaderValue.TryParseList([headerValue], out var parsed) || parsed is null)
        {
            return DefaultCulture;
        }

        var ranked = parsed
            .Where(v => v.Quality is null or > 0)
            .OrderByDescending(v => v.Quality ?? 1.0);

        foreach (var item in ranked)
        {
            var tag = item.Value.Value ?? string.Empty;

            var exact = SupportedCultures.FirstOrDefault(c =>
                string.Equals(c, tag, StringComparison.OrdinalIgnoreCase));
            if (exact is not null)
            {
                return exact;
            }

            var dash = tag.IndexOf('-');
            if (dash > 0)
            {
                var primary = tag[..dash];
                var primaryMatch = SupportedCultures.FirstOrDefault(c =>
                    string.Equals(c, primary, StringComparison.OrdinalIgnoreCase));
                if (primaryMatch is not null)
                {
                    return primaryMatch;
                }
            }
        }

        return DefaultCulture;
    }
}
