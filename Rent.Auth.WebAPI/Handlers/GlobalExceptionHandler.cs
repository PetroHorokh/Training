using Microsoft.AspNetCore.Diagnostics;
using Rent.Auth.DAL.CustomExceptions;
using Rent.Auth.WebAPI.ProblemDetails;

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
        CustomProblemDetails problemDetails;

        switch (exception)
        {
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

            case CredentialValidationException:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = exception.Message
                };
                break;

            case IdentityException:
                problemDetails = new CustomProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
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