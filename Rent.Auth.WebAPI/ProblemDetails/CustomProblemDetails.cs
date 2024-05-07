namespace Rent.Auth.WebAPI.ProblemDetails;

/// <summary>
/// Custom problem details class which add custom status code capabilities.
/// </summary>
public class CustomProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    /// <summary>
    /// Custom status code to include with httpContext for additional information
    /// </summary>
    public int? CustomStatusCode { get; set; }
}