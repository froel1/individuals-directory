using FluentValidation;
using IndividualsDirectory.Api.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace IndividualsDirectory.Api.Filters;

public class ValidationFilter(IStringLocalizer<Messages> localizer) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator) continue;

            var validationContext = (IValidationContext) Activator.CreateInstance(
                typeof(ValidationContext<>).MakeGenericType(argument.GetType()),
                argument)!;

            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = localizer["Validation_Failed"].Value,
                    Instance = context.HttpContext.Request.Path
                });
                return;
            }
        }

        await next();
    }
}