using Microsoft.AspNetCore.Identity;

namespace Rent.ExceptionLibrary;

/// <summary>
/// Exception thrown when IdentityResult has errors attached.
/// </summary>
[Serializable]
public class IdentityException : Exception
{
    /// <summary>
    /// Constructor with list of errors.
    /// </summary>
    /// <param name="errors">Parameter to add list of errors</param>
    public IdentityException(IEnumerable<IdentityError> errors)
    {
        Errors = errors;
    }

    /// <summary>
    /// Constructor with message and list of errors.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the exception</param>
    /// <param name="errors">Parameter to add list of errors</param>
    public IdentityException(string message, IEnumerable<IdentityError> errors)
        : base(message)
    {
        Errors = errors;
    }

    /// <summary>
    /// Constructor with message, inner exception and list of errors.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the custom exception</param>
    /// <param name="inner">Parameter to add inner exception message to the custom exception</param>
    /// <param name="errors">Parameter to add list of errors</param>
    public IdentityException(string message, Exception inner, IEnumerable<IdentityError> errors)
        : base(message, inner)
    {
        Errors = errors;
    }

    /// <summary>
    /// Property for errors returned by IdentityResult.
    /// </summary>
    public IEnumerable<IdentityError> Errors { get; set; }
}