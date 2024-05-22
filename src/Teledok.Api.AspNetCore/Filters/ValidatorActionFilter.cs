using Microsoft.AspNetCore.Mvc.Filters;
using Teledok.Core.Exceptions;

namespace Teledok.Api.AspNetCore.Filters;

public class ValidatorActionFilter : IAsyncActionFilter
{
    public async  Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid is false)
            throw new BadRequestException(context.ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage));

        await next();
    }
}