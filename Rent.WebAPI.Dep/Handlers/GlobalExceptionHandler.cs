using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Rent.ExceptionLibrary;
using Rent.WebAPI.ProblemDetails;

namespace Rent.WebAPI.Handlers;

/// <summary>
/// Handler for working with exception thrown in controllers.
/// </summary>
internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Interface implementation for working with thrown exceptions. Determines type of exception and customise details accordingly. Add details into httpContext. 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns boolean indicating if exception details were handled and attached to a httpContext</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        System.Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ArgumentException => new CustomProblemDetails
            {
                Status = StatusCodes.Status400BadRequest, Detail = exception.Message
            },
            ProcessException => new CustomProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError, Detail = exception.Message
            },
            NoEntitiesException => new CustomProblemDetails
            {
                Status = StatusCodes.Status204NoContent, Detail = exception.Message
            },
            ValidationException => new CustomProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity, Detail = exception.Message
            },
            AutoMapperMappingException => new CustomProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError, Detail = exception.Message
            },
            _ => new CustomProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError, Detail = exception.Message
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}