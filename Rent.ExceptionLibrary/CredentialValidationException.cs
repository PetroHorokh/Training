namespace Rent.ExceptionLibrary;

/// <summary>
/// Exception when credential verifying data were wrong.
/// </summary>
[Serializable]
public class CredentialValidationException : Exception
{
    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    public CredentialValidationException() { }

    /// <summary>
    /// Constructor with message.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the exception</param>
    public CredentialValidationException(string message)
        : base(message) { }

    /// <summary>
    /// Constructor with message and inner exception.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the custom exception</param>
    /// <param name="inner">Parameter to add inner exception message to the custom exception</param>
    public CredentialValidationException(string message, Exception inner)
        : base(message, inner) { }
}