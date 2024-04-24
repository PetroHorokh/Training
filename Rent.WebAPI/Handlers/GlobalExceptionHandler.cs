using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Rent.WebAPI.CustomExceptions;
using Rent.WebAPI.ProblemDetails;

namespace Rent.WebAPI.Handlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
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

            default:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Internal server error"
                };
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}