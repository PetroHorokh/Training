using Microsoft.AspNetCore.Diagnostics;
using Rent.Auth.WebAPI.ProblemDetails;
using Rent.ExceptionLibrary;

namespace Rent.Auth.WebAPI.Handlers;

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
        CustomProblemDetails problemDetails = exception switch
        {
            ProcessException => new CustomProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError, Detail = exception.Message
            },
            NoEntitiesException => new CustomProblemDetails
            {
                Status = StatusCodes.Status204NoContent, Detail = exception.Message
            },
            CredentialValidationException => new CustomProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized, Detail = exception.Message
            },
            IdentityException => new CustomProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized, Detail = exception.Message
            },
            _ => new CustomProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError, Detail = "Internal server error"
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}