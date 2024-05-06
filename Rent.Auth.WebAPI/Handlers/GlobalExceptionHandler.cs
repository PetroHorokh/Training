using Microsoft.AspNetCore.Diagnostics;
using Rent.Auth.DAL.CustomExceptions;
using Rent.Auth.WebAPI.ProblemDetails;

namespace Rent.Auth.WebAPI.Handlers;

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