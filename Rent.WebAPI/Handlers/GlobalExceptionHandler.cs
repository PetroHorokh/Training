using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Rent.WebAPI.CustomExceptions;
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
        Exception exception,
        CancellationToken cancellationToken)
    {
        CustomProblemDetails problemDetails;

        switch (exception)
        {
            case ArgumentException:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Detail = exception.Message
                };
                break;

            case ProcessException:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message
                };
                break;

            case NoEntitiesException:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status204NoContent,
                    Detail = exception.Message
                };
                break;

            case ValidationException:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = exception.Message
                };
                break;

            case AutoMapperMappingException:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message
                };
                break;

            default:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message
                };
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}