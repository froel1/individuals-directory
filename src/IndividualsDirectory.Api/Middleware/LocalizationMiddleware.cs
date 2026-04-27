using System.Globalization;

namespace IndividualsDirectory.Api.Middleware;

public class LocalizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var header = context.Request.Headers.AcceptLanguage.ToString();
        //for simplicity  (BCP 47 standard) isn't supported such as ka-GE or en-US
        var cultureCode = header
            .Split(',')
            .Any(tag => tag.Trim().StartsWith("ka", StringComparison.OrdinalIgnoreCase))
            ? "ka"
            : "en";

        var culture = new CultureInfo(cultureCode);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await next(context);
    }
}
