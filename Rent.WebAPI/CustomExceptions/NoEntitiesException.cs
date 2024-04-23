namespace Rent.WebAPI.CustomExceptions;

[Serializable]
public class NoEntitiesException : Exception
{
    public NoEntitiesException() { }

    public NoEntitiesException(string message)
        : base(message) { }

    public NoEntitiesException(string message, Exception inner)
        : base(message, inner) { }
}