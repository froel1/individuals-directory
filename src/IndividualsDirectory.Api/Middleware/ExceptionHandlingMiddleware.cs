using IndividualsDirectory.Api.Localization;
using IndividualsDirectory.Service.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace IndividualsDirectory.Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IStringLocalizer<Messages> localizer)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainValidationException ex)
        {
            if (context.Response.HasStarted) throw;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = localizer[ex.MessageKey, ex.Args].Value,
                Instance = context.Request.Path,
            });
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unhandled exception while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            if (context.Response.HasStarted) throw;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = localizer["Error_Unhandled"].Value,
                Instance = context.Request.Path,
            });
        }
    }
}
