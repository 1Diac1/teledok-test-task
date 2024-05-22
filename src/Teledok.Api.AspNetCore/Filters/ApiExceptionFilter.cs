﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Teledok.Core.Exceptions;


namespace Teledok.Api.AspNetCore.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    private static readonly IDictionary<Type, Action<ExceptionContext>> ExceptionHandlers =
        new Dictionary<Type, Action<ExceptionContext>>
        {
            [typeof(ArgumentException)] = HandleArgumentException,
            [typeof(ArgumentNullException)] = HandleArgumentException,
            [typeof(ArgumentOutOfRangeException)] = HandleArgumentException,
            [typeof(BadRequestException)] = HandleBadRequestException,
            [typeof(NotFoundException)] = HandleNotFoundException,
            [typeof(ValidationException)] = HandleFluentValidationException
        };

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private static void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();

        if (ExceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            return;
        }

        if (context.ModelState.IsValid is false)
            return;

        var details = new ValidationProblemDetails(context.ModelState);

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleArgumentException(ExceptionContext context)
    {
        var exception = (ArgumentException)context.Exception;

        var details = new ValidationProblemDetails()
        {
            Title = "Some error was occured",
            Status = 400
        };
        
        details.Extensions.Add("error", exception.Message);

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }
    
    private static void HandleBadRequestException(ExceptionContext context)
    {
        var exception = (BadRequestException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "Bad request",
            Status = 400
        };

        details.Extensions.Add("errors", exception.Errors);

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }
    
    private static void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "The specified resource was not found",
            Detail = exception.Message,
            Status = 404
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }
    
    private static void HandleFluentValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        
        var details = new ProblemDetails
        {
            Title = "Validation failed",
            Status = 400
        };
        
        details.Extensions.Add("errors", exception.Errors.Select(e => e.ErrorMessage));
        
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }
}