namespace Rent.WebAPI.CustomExceptions;

[Serializable]
public class ProcessException : Exception
{
    public ProcessException() { }

    public ProcessException(string message)
        : base(message) { }

    public ProcessException(string message, Exception inner)
        : base(message, inner) { }
}