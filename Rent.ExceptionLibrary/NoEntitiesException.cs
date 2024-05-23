namespace Rent.ExceptionLibrary;

/// <summary>
/// Exception when no entities were found.
/// </summary>
[Serializable]
public class NoEntitiesException : Exception
{
    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    public NoEntitiesException() { }

    /// <summary>
    /// Constructor with message.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the exception</param>
    public NoEntitiesException(string message)
        : base(message) { }

    /// <summary>
    /// Constructor with message and inner exception.
    /// </summary>
    /// <param name="message">Parameter to add explanation message to the custom exception</param>
    /// <param name="inner">Parameter to add inner exception message to the custom exception</param>
    public NoEntitiesException(string message, Exception inner)
        : base(message, inner) { }
}