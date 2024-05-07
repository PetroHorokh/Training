namespace Rent.WebAPI.CustomExceptions;

/// <summary>
/// Exception when internal service error has arisen.
/// </summary>
[Serializable]
public class ProcessException : Exception
{
    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    public ProcessException() { }

    /// <summary>
    /// Constructor with message.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the exception</param>
    public ProcessException(string message)
        : base(message) { }

    /// <summary>
    /// Constructor with message and inner exception.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the custom exception</param>
    /// <param name="inner">Parameter to add inner exception message to the custom exception</param>
    public ProcessException(string message, Exception inner)
        : base(message, inner) { }
}